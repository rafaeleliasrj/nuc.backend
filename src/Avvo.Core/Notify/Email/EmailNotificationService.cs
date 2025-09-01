using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Polly;
using Avvo.CoreNotify.Dto;
using Avvo.CoreNotify.Interface;
using Avvo.Core.Commons.Utils;
using Microsoft.Extensions.Logging;
using Avvo.CoreNotify.Enum;

namespace Avvo.CoreNotify.Email
{
    public partial class EmailNotificationService : IEmailNotificationService
    {
        private const string EMAIL_SUCCESS = "E-mail enviado com sucesso!";
        private const string EMAIL_ERROR = "Não foi possivel enviar o e-mail";
        private const string EMAIL_ERROR_EXCEPTION = "Não foi possivel enviar o email, segue mais detalhes do problema:";

        private readonly ILogger logger;
        public EmailNotificationService(ILogger logger)
        {
            this.logger = logger;
        }
        public async Task<EmailNotificationDtoResponse> SendEmail(EmailNotificationDto emailNotificationDto)
        {
            var retry = Policy.Handle<Exception>().WaitAndRetry(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            EmailNotificationDtoResponse emailNotificationResult = EmailNotificationDtoResponse.Create();
            try
            {
                await retry.Execute(async () => emailNotificationResult = await SendWithSES(emailNotificationDto));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "{0}.SendEmail Error", this.GetType().Name);
                throw;
            }

            return emailNotificationResult;
        }

        private async Task<EmailNotificationDtoResponse> SendWithSES(EmailNotificationDto emailNotificationDto)
        {
            using (var client = new AmazonSimpleEmailServiceClient())
            {
                try
                {
                    var responseEmail = (await client.SendEmailAsync(CreateObjectSES(emailNotificationDto)));

                    if (!string.IsNullOrEmpty(responseEmail.MessageId))
                    {
                        return EmailNotificationDtoResponse.Create(message: EMAIL_SUCCESS);
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "{0}.SendEmail Error {1}", this.GetType().Name, EMAIL_ERROR_EXCEPTION);
                    return EmailNotificationDtoResponse.Create(EMAIL_ERROR, ex, EmailStatusEnum.Error);
                }
            }

            return EmailNotificationDtoResponse.Create(EMAIL_ERROR);
        }

        private SendEmailRequest CreateObjectSES(EmailNotificationDto emailNotificationDto)
        {
            string sender = EnvironmentVariables.Get("EMAIL_SENDER");
            string configurationSetName = EnvironmentVariables.Get("EMAIL_CONFIGURATION_SET_NAME");
            return new SendEmailRequest
            {
                Source = sender,
                Destination = new Destination
                {
                    ToAddresses = emailNotificationDto.Receiver as List<string>
                },
                Message = new Message
                {
                    Subject = new Content(emailNotificationDto.Subject),
                    Body = SetBody(emailNotificationDto.HtmlBody, emailNotificationDto.TextBody)
                },
                ConfigurationSetName = configurationSetName
            };
        }

        private Body SetBody(string htmlBody, string textBody)
        {
            if (!string.IsNullOrEmpty(htmlBody))
            {
                return new Body
                {
                    Html = new Content
                    {
                        Charset = "UTF-8",
                        Data = htmlBody
                    }
                };
            }
            else
            {
                return new Body
                {
                    Html = new Content
                    {
                        Charset = "UTF-8",
                        Data = textBody
                    }
                };
            }
        }
    }
}
