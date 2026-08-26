using EngineeringAI.Application.Interfaces.Services;
using EngineeringAI.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application {
    public static class DependencyInjection {
        public static IServiceCollection AddApplication(this IServiceCollection services) {
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddValidatorsFromAssembly( typeof(DependencyInjection).Assembly, ServiceLifetime.Transient);
            services.AddScoped<EquipmentAiService>();
            services.AddScoped<EngineeringDocumentIngestionService>();
            
            return services;
        }
    }
}
