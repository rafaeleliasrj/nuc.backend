using System.Collections.Generic;

namespace Avvo.CoreNotify.Dto
{
    public class EmailNotificationDto
    {
        public IEnumerable<string> Receiver { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public static EmailNotificationDto Create(IEnumerable<string> receiver, string subject, string htmlBody = "", string textBody = "") => new EmailNotificationDto
        {
            Receiver = receiver,
            Subject = subject,
            HtmlBody = htmlBody,
            TextBody = textBody
        };
    }
}