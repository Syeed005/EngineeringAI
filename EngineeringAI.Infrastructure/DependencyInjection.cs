using Azure.Identity;
using Azure.Messaging.ServiceBus;
using EngineeringAI.Application.Interfaces.AI;
using EngineeringAI.Application.Interfaces.Messaging;
using EngineeringAI.Application.Interfaces.Repositories;
using EngineeringAI.Infrastructure.AI;
using EngineeringAI.Infrastructure.Data;
using EngineeringAI.Infrastructure.Messaging;
using EngineeringAI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure {
    public static class DependencyInjection {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
            
            services.AddDbContext<EngineeringDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("EngineeringDb")));
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddSingleton<IEngineeringAiClient, FoundryEngineeringAiClient>();

            services.AddSingleton(sp =>
            {
                var fullyQualifiedNamespace = configuration["ServiceBus:FullyQualifiedNamespace"];

                if (string.IsNullOrWhiteSpace(fullyQualifiedNamespace)) {
                    throw new InvalidOperationException("Service Bus namespace is not configured.");
                }

                return new ServiceBusClient(fullyQualifiedNamespace, new DefaultAzureCredential());
            });

            services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<ServiceBusClient>();
                return client.CreateSender("equipment-events");
            });

            services.AddScoped<IEquipmentEventPublisher, EquipmentEventPublisher>();

            return services;
        }
    }
}
