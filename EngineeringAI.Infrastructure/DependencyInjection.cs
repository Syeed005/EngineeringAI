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
            
            services.AddDbContext<EngineeringDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("EngineeringDb")));
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

            services.AddScoped<IEquipmentEventPublisher, EquipmentEventPublisher>();

            return services;
        }
    }
}
