using FluentEmail.Razor;
using FluentEmail.Smtp;
using Microsoft.Extensions.Logging;
using NautiHub.Core.Resources;
using NautiHub.CrossCutting.Services.Email.Interfaces;
using System.Diagnostics;

namespace NautiHub.CrossCutting.Services.Email.Providers;

public class LocalEmailService : IEmailService
{
    private readonly MessagesService _messagesService;
    private readonly ILogger<LocalEmailService> _logger;

    public LocalEmailService(MessagesService messagesService, ILogger<LocalEmailService> logger)
    {
        _messagesService = messagesService;
        _logger = logger;

        var pickupDir = Environment.GetEnvironmentVariable("EMAIL_LOCAL_PICKUP") ?? "Emails";
        if (!Path.IsPathRooted(pickupDir))
            pickupDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pickupDir);

        Directory.CreateDirectory(pickupDir);

        var sender = new SmtpSender(() =>
        {
            var client = new System.Net.Mail.SmtpClient()
            {
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = pickupDir
            };

            return client;
        });

        FluentEmail.Core.Email.DefaultSender = sender;
        FluentEmail.Core.Email.DefaultRenderer = new RazorRenderer();
    }

    public async Task SendEmailAsync(
    IEnumerable<string> recipients,
    string subject,
    string bodyHtml,
    IEnumerable<EmailAttachment>? attachments = null)
    {
        var email = FluentEmail.Core.Email
            .From("noreply@nautihub.com.br")
            .Subject(subject)
            .Body(bodyHtml, isHtml: true);

        foreach (var recipient in recipients)
        {
            email.To(recipient);
        }

        if (attachments is not null)
        {
            foreach (var attachment in attachments)
            {
                email.Attach(new FluentEmail.Core.Models.Attachment
                {
                    Data = new MemoryStream(attachment.Content),
                    Filename = attachment.FileName,
                    ContentType = attachment.MimeType
                });
            }
        }

        var response = await email.SendAsync();

        if (response.Successful)
            _logger.LogInformation(_messagesService.Email_Send_Success);
        else
            _logger.LogError($"{_messagesService.Email_Send_Failed}: {string.Join(", ", response.ErrorMessages)}");
    }
}
