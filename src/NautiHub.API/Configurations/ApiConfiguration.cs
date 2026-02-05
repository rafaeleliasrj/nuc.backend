using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NautiHub.API.Binders;
using NautiHub.API.Middlewares;
using NautiHub.Common.Middlewares;
using NautiHub.Core.Extensions;
using NautiHub.Core.Program;
using NautiHub.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NautiHub.API.Configurations;

internal static class ApiConfiguration
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new DelimitedListModelBinderProvider());
            })
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
                options.JsonSerializerOptions.IncludeFields = true;
            });

        services.AddMemoryCache();
        services.AddHealthChecks();
        services.ConfigureProblemDetailsModelState();

        services.AddSingleton(new InstanceIdentifier());

        services.AddCors(options =>
        {
            options.AddPolicy(
                "PoliticaCors",
                builder => builder.WithOrigins("http://localhost:3000", "http://localhost:3001", "https://localhost:3000", "https://localhost:3001")
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials()
                              .WithExposedHeaders("Content-Disposition")
            );
        });

        return services;
    }

    public static IApplicationBuilder UseApiConfiguration(
        this IApplicationBuilder app,
        IWebHostEnvironment env
    )
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors("PoliticaCors");

        app.UseMiddleware<RequestIdMiddleware>();
        app.UseRouting();

        //Ativa autentica��o bearer token.
        app.UseAuthentication();
        app.UseAuthorization();

        ILogger? logger = app.ApplicationServices.GetService<ILogger<Program>>();
        app.UseProblemDetailsExceptionHandler(logger);

        app.UseWhen(
            context => context.GetEndpoint()?.Metadata.GetMetadata<IAuthorizeData>() != null,
            appBuilder => appBuilder.UseMiddleware<AutenticacaoMiddleware>()
        );

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Nauti Hub API v1");
                options.RoutePrefix = "swagger";
            });
        }

        return app;
    }
}
