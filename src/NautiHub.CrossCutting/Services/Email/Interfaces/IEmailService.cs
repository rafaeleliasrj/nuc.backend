namespace NautiHub.CrossCutting.Services.Email.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(
        IEnumerable<string> recipients,
        string subject,
        string bodyHtml,
        IEnumerable<EmailAttachment> attachments = null);
}

public record EmailAttachment(
    string FileName,
    byte[] Content,
    string MimeType);
