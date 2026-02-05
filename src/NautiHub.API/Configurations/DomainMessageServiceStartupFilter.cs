using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NautiHub.Core.Resources;
using NautiHub.Domain.Services;

namespace NautiHub.API.Configurations;

/// <summary>
/// StartupFilter para configurar o DomainMessageService com o MessagesService
/// Mantém a pureza do domínio enquanto permite injeção de dependências
/// </summary>
public class DomainMessageServiceStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            // Configurar o DomainMessageService com a instância do MessagesService
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var messagesService = scope.ServiceProvider.GetRequiredService<MessagesService>();
                DomainMessageService.Configure(messagesService);
            }

            next(app);
        };
    }
}