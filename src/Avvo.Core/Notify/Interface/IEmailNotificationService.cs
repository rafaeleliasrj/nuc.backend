using System.Threading.Tasks;
using Avvo.CoreNotify.Dto;

namespace Avvo.CoreNotify.Interface
{
    public partial interface IEmailNotificationService
    {
        Task<EmailNotificationDtoResponse> SendEmail(EmailNotificationDto emailNotificationDto);
    }
}
