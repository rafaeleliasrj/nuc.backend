using System.Collections.Generic;

namespace Avvo.CoreNotify.Dto
{
    public class SMSNotificationResponseDto
    {
        public string MessageId { get; set; }

        public static SMSNotificationResponseDto Create(string messageId) => new SMSNotificationResponseDto
        {
            MessageId = messageId
        };
    }
}