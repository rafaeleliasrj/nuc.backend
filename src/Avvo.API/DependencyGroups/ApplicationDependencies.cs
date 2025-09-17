using System.Text.Json;
using System.Text.Json.Serialization;
using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Utils;
using Avvo.Core.Data.EntityFramework.EventProcess;
using Avvo.Core.Host.DependencyGroups;
using Avvo.Core.Logging;
using Avvo.Core.Logging.Correlation;
using Avvo.Core.Messaging.Publisher;
using FluentValidation;
using FluentValidation.AspNetCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Avvo.API.DependencyGroups;

/// <summary>
///     Application Dependencies Class
/// </summary>
public class ApplicationDependencies : IDependencyGroup
{
    /// <summary>
    ///     Register Dependencies
    /// </summary>
    /// <param name="logger">Logger Object</param>
    /// <param name="serviceCollection">Service Collection Object</param>
    public void Register(ILogger logger, IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ICorrelationService, CorrelationService>();
        serviceCollection.AddTransient<ITraceService, TraceService>();
        serviceCollection.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.FloatFormatHandling =
                    FloatFormatHandling.DefaultValue;
                options.SerializerSettings.FloatParseHandling = FloatParseHandling.Decimal;
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                options.SerializerSettings.Converters.Add(new DateTimeJsonConverter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
        serviceCollection.AddHealthChecks();
        serviceCollection.AddCors();
        serviceCollection.AddValidatorsFromAssemblyContaining<Program>();
        serviceCollection.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        serviceCollection.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        });
        serviceCollection.AddControllersWithViews().AddNewtonsoftJson();

        serviceCollection.AddTransient<ISqsPublisher<CrudEventMessage>>(services =>
            {
                var correlationService = services.GetService<ICorrelationService>();
                return new SqsPublisher<CrudEventMessage>(correlationService)
                {
                    QueueName = EnvironmentVariables.Get("CRUD_EVENT_QUEUE_NAME")
                };
            });

        serviceCollection.AddScoped<ProcessorCrudEvent, ProcessorCrudEvent>();
    }
}
