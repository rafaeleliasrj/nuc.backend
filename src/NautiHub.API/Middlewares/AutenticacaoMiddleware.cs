using NautiHub.Core.Resources;
using NautiHub.Domain.Services.DomainService;
using System.Text.Json;

namespace NautiHub.API.Middlewares;

public class AutenticacaoMiddleware(RequestDelegate next, MessagesService messagesService)
{
    private readonly RequestDelegate _next = next;
    private readonly MessagesService _messagesService = messagesService;

    private async Task ErrorMessage(HttpContext context, string message, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var erro = new
        {
            errors = new { Mensagens = new[] { message } },
            title = _messagesService.Error_Validation_Title,
            status = statusCode
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(erro));
    }

    private Task Unauthorized(HttpContext context, string mensagem) =>
        ErrorMessage(context, mensagem, StatusCodes.Status401Unauthorized);

    private Task BadRequest(HttpContext context, string mensagem) =>
        ErrorMessage(context, mensagem, StatusCodes.Status400BadRequest);

    private Task NotFound(HttpContext context, string mensagem) =>
        ErrorMessage(context, mensagem, StatusCodes.Status404NotFound);

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        Guid userId = authService.GetUserId();

        if (userId == Guid.Empty && authService.HasValidateUser())
        {
            await Unauthorized(context, _messagesService.Auth_User_Invalid);
            return;
        }

        await _next(context);
    }
}
