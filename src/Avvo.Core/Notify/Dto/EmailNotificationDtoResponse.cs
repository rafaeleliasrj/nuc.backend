using System;
using Avvo.CoreNotify.Enum;

namespace Avvo.CoreNotify.Dto
{
    public class EmailNotificationDtoResponse
    {
        public EmailStatusEnum ResultStatus { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public static EmailNotificationDtoResponse Create(string message = "", Exception exception = null, EmailStatusEnum resultStatus = EmailStatusEnum.Success) => new EmailNotificationDtoResponse
        {
            ResultStatus = resultStatus,
            Exception = exception,
            Message = message
        };
    }
}