using Amazon.S3;
using Amazon.SQS;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using NautiHub.Common.Configurations;
using NautiHub.Common.Middlewares.Services;
using NautiHub.Core.Aws;
using NautiHub.Core.Behaviors;
using NautiHub.Core.DomainObjects;
using NautiHub.Core.Extensions;
using NautiHub.CrossCutting.Services.Idempotency;
using NautiHub.Core.Mediator;
using NautiHub.Core.Messages.Features;
using NautiHub.CrossCutting.Services.Email.Interfaces;
using NautiHub.CrossCutting.Services.Email.Providers;
using NautiHub.CrossCutting.Services.Templates.Interfaces;
using NautiHub.CrossCutting.Services.Templates.Providers;
using NautiHub.Domain.Repositories;
using NautiHub.Domain.Services;
using NautiHub.Domain.Services.DomainService;
using NautiHub.Domain.Services.InfrastructureService.Cache;
using NautiHub.Domain.Services.InfrastructureService.File;
using NautiHub.Domain.Services.InfrastructureService.Ibge;
using NautiHub.Domain.Services.InfrastructureService.ServidorDeMensagem;
using NautiHub.Infrastructure.DataContext;
using NautiHub.Infrastructure.Repositories;
using NautiHub.CrossCutting.Services.Cache;
using NautiHub.CrossCutting.Services.File;
using NautiHub.Infrastructure.Services.Message;
using NautiHub.Infrastructure.Services.Message.Servers.Meta;
using NautiHub.Infrastructure.Services.Message.Servers.Twilio;
using System.Reflection;
using System.Runtime.InteropServices;
using NautiHub.Infrastructure.Services.Comum.ServidorIbge;
using NautiHub.CrossCutting.Services.Cache.Operators.MemoryCache;
using NautiHub.CrossCutting.Services.Cache.Operators.Redis;
using NautiHub.CrossCutting.Services.File.Operators.S3Bucket;
using NautiHub.Infrastructure.Services.File;
using NautiHub.Infrastructure.Services.Cache;
using NautiHub.Core.Resources;
using NautiHub.Application.Configurations;
using NautiHub.Application.Services;
using NautiHub.Infrastructure.Gateways.Asaas;

namespace NautiHub.API.Configurations
{
    public static class CommonServicesConfiguration
    {
        public static void AddCommonServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            Assembly assembly = AppDomain.CurrentDomain.Load("NautiHub.Application");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string basePath = AppContext.BaseDirectory;
                string dllPath = Path.Combine(basePath, "libwkhtmltox.dll");

                if (!File.Exists(dllPath))
                    throw new FileNotFoundException($"DLL não encontrada: {dllPath}");

                var context = new CustomAssemblyLoadContext();
                context.LoadUnmanagedLibrary(dllPath);
            }
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(assembly);
            services.AddFluentValidationAutoValidation();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));

            // AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<IMappingService, MappingService>();

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(FeatureHandlerValidation<,>)
            );

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddDbContext<DatabaseContext>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<IClientAws, ClientAws>();

            // AWS SQS Client
            services.AddSingleton(provider =>
            {
                Console.WriteLine("Iniciando AmazonSQS.");
                IClientAws apiClientAws = provider.GetRequiredService<IClientAws>();
                var localstackEndpoint = Environment.GetEnvironmentVariable("LOCALSTACK_ENDPOINT");

                if (!string.IsNullOrEmpty(localstackEndpoint))
                {
                    var config = new AmazonSQSConfig { ServiceURL = localstackEndpoint, UseHttp = true };
                    return new AmazonSQSClient(config);
                }
                return apiClientAws.GetClientAmazonSQS();
            });

            // AWS S3 Client
            services.AddSingleton(provider =>
            {
                Console.WriteLine("Iniciando AmazonS3.");
                IClientAws apiClientAws = provider.GetRequiredService<IClientAws>();
                var localstackEndpoint = Environment.GetEnvironmentVariable("LOCALSTACK_ENDPOINT");
                if (!string.IsNullOrEmpty(localstackEndpoint))
                {
                    var config = new AmazonS3Config { ServiceURL = localstackEndpoint, UseHttp = true, ForcePathStyle = true };
                    return new AmazonS3Client(config);
                }
                return apiClientAws.GetClientAmazonS3();
            });

            //Autenticacao
            services.AddScoped<IAuthService, AuthService>();

            //Identity
            services.AddHttpContextAccessor();
            services.AddScoped<INautiHubIdentity>(provider =>
            {
                HttpContext? context = provider.GetService<IHttpContextAccessor>()?.HttpContext;
                ILogger<NautiHubIdentity> logger = provider.GetRequiredService<ILogger<NautiHubIdentity>>();
                HttpRequest? req = context?.Request;
                var requestId = req?.Headers["x-request-id"].ToString();
                var userIp = req?.Headers["x-forwarded-for"].ToString();
                IEnumerable<System.Security.Claims.Claim>? claims = context?.User?.Claims;

                if (claims != null && claims.Any())
                {
                    NautiHubIdentity? identity = claims.GetIdentity<NautiHubIdentity>(logger);

                    if (!string.IsNullOrEmpty(requestId))
                        identity?.SetRequestId(Guid.Parse(requestId));

                    if (!string.IsNullOrEmpty(userIp))
                        identity?.SetUserIp(userIp);

                    return identity ?? new NautiHubIdentity();
                }

                return new NautiHubIdentity();
            });

            //Email
            var emailProvider = Environment.GetEnvironmentVariable("OPERADOR_ENVIO_EMAIL") + "";

            if (string.IsNullOrWhiteSpace(emailProvider))
                emailProvider = EmailProviderEnum.Local.ToString().ToLower();
            else
                emailProvider = emailProvider.ToLower();

            if (emailProvider == EmailProviderEnum.SendGrid.ToString().ToLower())
                services.AddSingleton<IEmailService>(provider => new SendGridEmailService(
                    Environment.GetEnvironmentVariable("SENDGRID_API_KEY") + "",
                    provider.GetRequiredService<MessagesService>(),
                    provider.GetRequiredService<ILogger<SendGridEmailService>>()
                ));
            else
                services.AddSingleton<IEmailService>(provider => new LocalEmailService(
                    provider.GetRequiredService<MessagesService>(),
                    provider.GetRequiredService<ILogger<LocalEmailService>>()
                ));

            //Templates
            services.AddSingleton<RazorTemplateService>();

            services.AddSingleton<ITemplateService>(provider => new NautiHub.CrossCutting.Services.Templates.TemplateService(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates"),
                provider.GetRequiredService<RazorTemplateService>(),
                provider.GetRequiredService<MessagesService>(),
                provider.GetRequiredService<ILogger<NautiHub.CrossCutting.Services.Templates.TemplateService>>()
            ));

            //Diversos
            services.AddScoped<IIdempotencyService, NautiHub.CrossCutting.Services.Idempotency.IdempotencyService>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));
            services.AddHttpClient();

            //Repository - Domain
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBoatRepository, BoatRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IScheduledTourRepository, ScheduledTourRepository>();

            
            //Services
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IIbgeService, ServidorIbgeService>();

            // Asaas Payment Gateway
            services.Configure<AsaasSettings>(configuration.GetSection("Asaas"));
            services.AddScoped<IAsaasService, AsaasService>();

            //Strategy
            services.AddScoped<RedisCacheStrategy>();
            services.AddScoped<MemoryCacheStrategy>();
            services.AddScoped<S3BucketStrategy>();
            services.AddScoped<TwilioStrategy>();
            services.AddScoped<MetaStrategy>();

            //Factory
            services.AddScoped<ICacheStrategyFactory, CacheStrategyFactory>();
            services.AddScoped<IFileStrategyFactory, FileStrategyFactory>();
            services.AddScoped<IMessageStrategyFactory, MessageStrategyFactory>();
            
            //Localization
            services.AddLocalization();
            services.AddTransient<MessagesService>();
            services.AddSingleton<IDomainMessageService, DomainMessageService>();
            
            // Configure DomainMessageService with MessagesService after build
            services.AddSingleton<IStartupFilter>(new DomainMessageServiceStartupFilter());
        }
    }
}
