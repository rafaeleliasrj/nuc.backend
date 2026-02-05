using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NautiHub.Core.DomainObjects;
using NautiHub.Core.Resources;
using System.Text.Json;

namespace NautiHub.Core.Extensions;

public static class ProblemDetailsHandlerExtension
{
    public static void UseProblemDetailsExceptionHandler(
        this IApplicationBuilder app,
        ILogger? logger = null
    )
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async contexto =>
            {
                // Obter o MessagesService do serviço provider
                using var scope = contexto.RequestServices.CreateScope();
                var messagesService = scope.ServiceProvider.GetRequiredService<MessagesService>();
                
                IExceptionHandlerFeature? exceptionHandlerFeature = contexto.Features.Get<IExceptionHandlerFeature>();

                if (exceptionHandlerFeature != null)
                {
                    Exception excecao = exceptionHandlerFeature.Error;

                    var problemDetails = new ProblemDetails
                    {
                        Instance = contexto.Request.HttpContext.Request.Path
                    };

                    string chave = "";

                    if (contexto.Request.Headers.TryGetValue("X-Contract-Key", out Microsoft.Extensions.Primitives.StringValues contractKey))
                        chave = $"Chave de contrato - {contractKey}";
                    else if (contexto.Request.Headers.TryGetValue("X-Api-Key", out Microsoft.Extensions.Primitives.StringValues apiKey))
                        chave = $"Chave de integração - {apiKey}";

                    if (logger != null)
                        logger.LogError($"{chave} - {excecao.Message}");

                    contexto.Response.ContentType = "application/problem+json";

                    if (excecao is DomainException domainException)
                    {
                        // Tentar obter a mensagem localizada usando reflexão
                        string errorMessage = domainException.Message;
                        try
                        {
                            var messageKeyProperty = domainException.GetType().GetProperty("MessageKey");
                            if (messageKeyProperty?.GetValue(domainException) is string messageKey && !string.IsNullOrEmpty(messageKey))
                            {
                                var messageProperty = messagesService.GetType().GetProperty(messageKey);
                                if (messageProperty?.GetValue(messagesService) is string localizedMessage)
                                {
                                    errorMessage = localizedMessage;
                                }
                            }
                        }
                        catch
                        {
                            // Se não conseguir obter a mensagem localizada, usa a padrão
                        }

                        var modelErro = new ModelStateDictionary();
                        modelErro.AddModelError("Mensagens", errorMessage);

                        var validation = new ValidationProblemDetails(modelErro)
                        {
                            Instance = contexto.Request.HttpContext.Request.Path,
                            Title = messagesService.Error_Validation_Title,
                            Status = (int)domainException.StatusCode
                        };

                        contexto.Response.StatusCode = validation.Status.Value;

                        var json = JsonSerializer.Serialize(validation);

                        await contexto.Response.WriteAsync(json);
                    }
                    else if (excecao is ValidationException validationException)
                    {
                        var errosValidacao = validationException
                            .Errors.Select(c => c.ErrorMessage)
                            .ToArray();

                        var modelErro = new ModelStateDictionary();
                        foreach (var erro in errosValidacao)
                        {
                            modelErro.AddModelError("Mensagens", erro);
                        }

                        var validation = new ValidationProblemDetails(modelErro)
                        {
                            Instance = contexto.Request.HttpContext.Request.Path,
                            Title = messagesService.Error_Validation_Title,
                            Status = 400
                        };

                        contexto.Response.StatusCode = validation.Status.Value;

                        var json = JsonSerializer.Serialize(validation);

                        await contexto.Response.WriteAsync(json);
                    }
                    else if (excecao is UnauthorizedAccessException unauthorizedException)
                    {
                        problemDetails.Title = messagesService.Error_Not_Authorized_Title;
                        problemDetails.Status = StatusCodes.Status401Unauthorized;
                        problemDetails.Detail = excecao.Message;

                        contexto.Response.StatusCode = problemDetails.Status.Value;

                        var json = JsonSerializer.Serialize(problemDetails);

                        await contexto.Response.WriteAsync(json);
                    }
                    else
                    {
                        problemDetails.Title = messagesService.Error_Internal_Server;
                        problemDetails.Status = StatusCodes.Status500InternalServerError;
                        problemDetails.Detail = excecao.Message;

                        contexto.Response.StatusCode = problemDetails.Status.Value;

                        var json = JsonSerializer.Serialize(problemDetails);

                        await contexto.Response.WriteAsync(json);
                    }
                }
            });
        });
    }

    public static IServiceCollection ConfigureProblemDetailsModelState(
        this IServiceCollection services
    )
    {
        return services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                // Obter o MessagesService do serviço provider
                using var scope = context.HttpContext.RequestServices.CreateScope();
                var messagesService = scope.ServiceProvider.GetRequiredService<MessagesService>();
                
                var erros = context.ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = messagesService.Error_Bad_Request_Message
                };

                problemDetails.Errors.Clear();
                problemDetails.Errors.Add("Mensagens", erros);

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json", "application/problem+xml" }
                };
            };
        });
    }
}