using EngineeringAI.Application.Interfaces.Repositories;
using EngineeringAI.Infrastructure.Data;
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
            services.AddDbContext<EngineeringDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EngineeringDb")));

            services.AddScoped<IEquipmentRepository, EquipmentRepository>();

            return services;
        }
    }
}
