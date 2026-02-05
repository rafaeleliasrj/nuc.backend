using Microsoft.Extensions.Logging;
using NautiHub.Core.Resources;
using NautiHub.CrossCutting.Services.Email.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;

namespace NautiHub.CrossCutting.Services.Email.Providers;

public class SendGridEmailService : IEmailService
{
    private readonly SendGridClient _emailClient;
    private readonly HttpClient _httpClient;
    private readonly MessagesService _messagesService;
    private readonly ILogger<SendGridEmailService> _logger;

    public SendGridEmailService(string apiKey, MessagesService messagesService, ILogger<SendGridEmailService> logger)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException(
                _messagesService.Email_SendGrid_Key_Required,
                nameof(apiKey)
            );

        this._messagesService = messagesService;
        this._logger = logger;
        this._httpClient = new HttpClient();
        this._emailClient = new SendGridClient(_httpClient,
            new SendGridClientOptions { ApiKey = apiKey, HttpErrorAsException = true });
    }

    public async Task SendEmailAsync(
    IEnumerable<string> recipients,
    string subject,
    string bodyHtml,
    IEnumerable<EmailAttachment>? attachments = null)
    {
        var from = new EmailAddress("noreply@nautihub.com.br", "Nauti Hub");
        var tos = recipients.Select(email => new EmailAddress(email)).ToList();

        var msg = MailHelper.CreateSingleEmailToMultipleRecipients(
            from,
            tos,
            subject,
            plainTextContent: "",
            htmlContent: bodyHtml
        );

        if (attachments is not null)
        {
            foreach (var attachment in attachments)
            {
                var base64 = Convert.ToBase64String(attachment.Content);
                msg.AddAttachment(attachment.FileName, base64, attachment.MimeType);
            }
        }

        var response = await _emailClient.SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
            _logger.LogInformation(_messagesService.Email_Send_Success);
        else
            _logger.LogError(_messagesService.Email_Send_Failed);
    }
}
