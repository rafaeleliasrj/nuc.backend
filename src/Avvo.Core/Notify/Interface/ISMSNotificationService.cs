using System.Threading.Tasks;
using Avvo.CoreNotify.Dto;

namespace Avvo.CoreNotify.Interface
{
    public interface ISMSNotificationService
    {
        Task<SMSNotificationResponseDto> Execute(SMSNotificationRequestDto smsNotificationRequest);
    }
}