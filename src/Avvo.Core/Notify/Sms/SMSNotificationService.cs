using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Avvo.Core.Commons.Exceptions;
using Avvo.CoreNotify.Dto;
using Avvo.CoreNotify.Interface;
using Microsoft.Extensions.Logging;

namespace Avvo.CoreNotify.Sms
{
    public class SMSNotificationService : ISMSNotificationService
    {
        private readonly ILogger logger;
        public SMSNotificationService(ILogger logger)
        {
            this.logger = logger;
        }
        public async Task<SMSNotificationResponseDto> Execute(SMSNotificationRequestDto smsNotificationRequest)
        {
            if (string.IsNullOrEmpty(smsNotificationRequest.Message))
                throw new HttpStatusException(HttpStatusCode.PreconditionFailed, "Message is required.");

            if (string.IsNullOrEmpty(smsNotificationRequest.PhoneNumber))
                throw new HttpStatusException(HttpStatusCode.PreconditionFailed, "Phone Number is required.");

            var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(smsNotificationRequest.Message);

            if (size > 140)
                throw new HttpStatusException((HttpStatusCode)422, "Message exceeds the maximum allowed");



            using (var client = new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.USEast1))
            {
                var request = new PublishRequest
                {
                    Message = smsNotificationRequest.Message,
                    PhoneNumber = smsNotificationRequest.PhoneNumber,
                };

                try
                {
                    var response = await client.PublishAsync(request);

                    logger.LogInformation("{0}.Execute Message sent to {1}: {2}", GetType().Name, smsNotificationRequest.PhoneNumber, smsNotificationRequest.Message);

                    if (response.HttpStatusCode != HttpStatusCode.OK)
                    {
                        var error = new HttpStatusException(response.HttpStatusCode, "Error message sent ");
                        this.logger.LogError(error, "{0}.Execute", this.GetType().Name);
                        throw error;
                    }

                    return SMSNotificationResponseDto.Create(response.MessageId);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{0}.Execute Error message sent to {1}: {2}", GetType().Name, smsNotificationRequest.PhoneNumber, smsNotificationRequest.Message);
                    throw;
                }
            }
        }
    }
}
