using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;
using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Host.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {

        /// <summary>
        /// Middleware utilizado para capturar/tratar as exceções não tratadas ou disparadas de forma intencional pelas aplicações.
        /// </summary>
        /// <param name="app"></param>
        [Obsolete("Utilize o UseAvvoExceptionHandler.", false)]
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = HandleExceptionWithoutLogging
            });
        }

        /// <summary>
        /// Middleware utilizado para capturar/tratar as exceções não tratadas ou disparadas de forma intencional pelas aplicações.
        /// Caso identifique que está ocorrendo log em duplicidade, verifique se foi adicionado o filtro abaixo no LogginBuilder:
        /// <example><code>
        /// builder.AddFilter("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogLevel.None);
        ///</code></example>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="shouldLogException">
        /// Define se as exceptions capturadas devem ser logadas (de acordo com o LogLevel definido na ExceptionBase) ou apenas tratar para gerar o retorno ErrorDetails
        /// </param>
        public static void UseAvvoExceptionHandler(this IApplicationBuilder app, bool shouldLogException = true)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = shouldLogException ? HandleExceptionWithLogging : HandleExceptionWithoutLogging
            });
        }

        private static Task HandleExceptionWithoutLogging(HttpContext context)
        {
            return ExecuteHandleException(context, false);
        }

        private static Task HandleExceptionWithLogging(HttpContext context)
        {
            return ExecuteHandleException(context, true);
        }

        private static async Task ExecuteHandleException(HttpContext context, bool shouldLogException)
        {
            context.Response.ContentType = context.Request.ContentType;
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

            if (contextFeature != null)
            {
                var errorDetails = GetErrorDetails(contextFeature);

                if (shouldLogException)
                    ExecuteLog(context, contextFeature, errorDetails);

                context.Response.StatusCode = (int)errorDetails.StatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(errorDetails.ToString());
            }
        }

        private static void ExecuteLog(HttpContext context, IExceptionHandlerFeature contextFeature, ErrorDetails errorDetails)
        {
            try
            {
                if (contextFeature is not null && contextFeature.Error is not null)
                {
                    ILogger logger = context.RequestServices.GetService<ILogger>();
                    LogLevel logLevel = (contextFeature.Error as ExceptionBase)?.LogLevel ?? LogLevel.Error;

                    if (contextFeature is IExceptionHandlerPathFeature exceptionHandlerPathFeature)
                        contextFeature.Error.Data.Add("RequestPath", exceptionHandlerPathFeature.Path);

                    if ((context.Request.QueryString.HasValue))
                        contextFeature.Error.Data.Add("RequestQueryString", context.Request.QueryString.Value);

                    if (!string.IsNullOrEmpty(context.Request.Headers["x-api-key"]))
                    {
                        var xApiKey = $"{context.Request.Headers["x-api-key"]}";
                        int charNotEncode = xApiKey.Length / 2;
                        xApiKey = xApiKey.Substring(charNotEncode).PadLeft(xApiKey.Length, '*');
                        contextFeature.Error.Data.Add("RequestApiKey", xApiKey);
                    }

                    contextFeature.Error.Data.Add("ResponseErrorDetails", errorDetails);

                    if (logLevel is not LogLevel.None && logger.IsEnabled(logLevel))
                        logger?.Log(logLevel, contextFeature.Error, contextFeature.Error.Message);
                }
            }
            catch (System.Exception ex)
            {
                //utiliza Console.Write por possíveis erros na instância do ILogger
                Console.WriteLine($"Ocorreu um ao erro tentar efetuar o log. Message: {ex.Message}. StackTrace: {ex.StackTrace}");
            }
        }

        private static ErrorDetails GetErrorDetails(IExceptionHandlerFeature contextFeature)
        {
            var error = contextFeature.Error;
            var errorDetails = ErrorDetails.Create();
            errorDetails.StatusCode = GetErrorCode(error);

            if (error is FluentValidation.ValidationException validationException)
            {
                if (validationException.Errors.Any())
                    errorDetails.Messages.AddRange(validationException.Errors.Select(err => err.ErrorMessage));
                else
                    errorDetails.Messages.Add(validationException.Message);
            }
            else if (error is HttpStatusException httpStatusException)
            {
                errorDetails.StatusCode = httpStatusException.StatusCode;
                errorDetails.ErrorCode = httpStatusException.ErrorCode;
                errorDetails.Messages.Add($"{httpStatusException.Message}");
            }
            else if (error is NotFoundException notFoundException)
            {
                errorDetails.StatusCode = HttpStatusCode.NotFound;
            }
            else if (error is Exception exception)
            {
                errorDetails.Messages.Add(exception.Message);
            }
            else
            {
                errorDetails.ErrorCode = "INTERNAL_SERVER_ERROR";
                errorDetails.Messages.Add(contextFeature.Error.Message);
            }
            return errorDetails;
        }

        private static HttpStatusCode GetErrorCode(Exception e)
        {
            switch (e)
            {
                case FluentValidation.ValidationException _:
                    return HttpStatusCode.PreconditionFailed;
                case ValidationException _:
                    return HttpStatusCode.BadRequest;
                case FormatException _:
                    return HttpStatusCode.BadRequest;
                case AuthenticationException _:
                    return HttpStatusCode.Forbidden;
                case NotImplementedException _:
                    return HttpStatusCode.NotImplemented;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}
