using FluentEmail.Razor;
using FluentEmail.Smtp;
using Microsoft.Extensions.Logging;
using NautiHub.Core.Resources;
using NautiHub.CrossCutting.Services.Email.Interfaces;
using System.Diagnostics;

namespace NautiHub.CrossCutting.Services.Email.Providers;

public class MailJetEmailService : IEmailService
{
    private readonly MessagesService _messagesService;
    private readonly ILogger<MailJetEmailService> _logger;

    public MailJetEmailService(MessagesService messagesService, ILogger<MailJetEmailService> logger)
    {
        _messagesService = messagesService;
        _logger = logger;

        var sender = new SmtpSender(() =>
        {
            var client = new System.Net.Mail.SmtpClient()
            {
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = Environment.GetEnvironmentVariable("EMAIL_LOCAL_PICKUP")! ?? ""
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
        try
        {
            var email = FluentEmail.Core.Email
                .From("noreply@nautihubi.com.br")
                .To(string.Join(",", recipients))
                .Subject(subject)
                .Body(bodyHtml, isHtml: true);

            if (attachments is not null)
            {
                foreach (var attachment in attachments)
                {
                    var stream = new MemoryStream(attachment.Content);
                    email.Attach(new FluentEmail.Core.Models.Attachment
                    {
                        Data = stream,
                        Filename = attachment.FileName,
                        ContentType = attachment.MimeType
                    });
                }
            }

            var response = await email.SendAsync();

            if (response.Successful)
            {
                _logger.LogInformation(_messagesService.Email_Send_Success);
                return;
            }

            _logger.LogError(_messagesService.Email_Send_Failed);
            foreach (var error in response.ErrorMessages)
                _logger.LogError($"- {error}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _messagesService.Email_Send_Error);
        }
    }
}
