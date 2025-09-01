using System.Collections.Generic;

namespace Avvo.CoreNotify.Dto
{
    public class SMSNotificationRequestDto
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }

        public static SMSNotificationRequestDto Create(string phoneNumber, string message) => new SMSNotificationRequestDto
        {
            PhoneNumber = phoneNumber,
            Message = message
        };
    }
}