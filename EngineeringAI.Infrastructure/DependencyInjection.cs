using Azure.Identity;
using Azure.Messaging.ServiceBus;
using EngineeringAI.Application.Interfaces.AI;
using EngineeringAI.Application.Interfaces.Messaging;
using EngineeringAI.Application.Interfaces.Repositories;
using EngineeringAI.Application.Options;
using EngineeringAI.Infrastructure.AI;
using EngineeringAI.Infrastructure.Data;
using EngineeringAI.Infrastructure.Messaging;
using EngineeringAI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure {
    public static class DependencyInjection {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {

            var engineeringDbConnectionString = configuration.GetConnectionString("EngineeringDb");

            if (string.IsNullOrWhiteSpace(engineeringDbConnectionString)) {
                throw new InvalidOperationException("EngineeringDb connection string is not configured.");
            }

            services.AddDbContext<EngineeringDbContext>(options => options.UseSqlServer(engineeringDbConnectionString));
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddSingleton<IEngineeringAiClient, FoundryEngineeringAiClient>();

            services.AddOptions<ServiceBusOptions>()
                .Bind(configuration.GetSection(ServiceBusOptions.SectionName))
                .Validate(
                    options => !string.IsNullOrWhiteSpace(options.FullyQualifiedNamespace),
                    "Service Bus namespace is required.")
                .Validate(
                    options => !string.IsNullOrWhiteSpace(options.EquipmentEventsQueue),
                    "Service Bus equipment events queue is required.")
                .ValidateOnStart();


            services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;

                return new ServiceBusClient(
                    options.FullyQualifiedNamespace,
                    new DefaultAzureCredential());
            });

            services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<ServiceBusClient>();
                var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;

                return client.CreateSender(options.EquipmentEventsQueue);
            });

            services.AddOptions<AiOptions>()
                .Bind(configuration.GetSection(AiOptions.SectionName))
                .Validate(
                    options => !string.IsNullOrWhiteSpace(options.Endpoint),
                    "AI endpoint is required.")
                .Validate(
                    options => Uri.TryCreate(
                        options.Endpoint,
                        UriKind.Absolute,
                        out var uri) &&
                        (uri.Scheme == Uri.UriSchemeHttps ||
                         uri.Scheme == Uri.UriSchemeHttp),
                    "AI endpoint must be a valid HTTP/HTTPS URI.")
                .Validate(
                    options => !string.IsNullOrWhiteSpace(options.Deployment),
                    "AI deployment is required.")
                .ValidateOnStart();

            services.AddScoped<IEquipmentEventPublisher, EquipmentEventPublisher>();
            services.AddSingleton<IEngineeringAiClient, FoundryEngineeringAiClient>();

            return services;
        }
    }
}
