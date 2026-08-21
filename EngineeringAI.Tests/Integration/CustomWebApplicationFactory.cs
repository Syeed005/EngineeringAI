using EngineeringAI.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Tests.Integration {
    public class CustomWebApplicationFactory : WebApplicationFactory<Program> {
        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<EngineeringDbContext>>();
                services.RemoveAll<EngineeringDbContext>();

                services.AddDbContext<EngineeringDbContext>(options =>
                    options.UseSqlServer(
                        "Server=RAHMAN-2024001;Database=EngineeringAI_Test;Trusted_Connection=True;TrustServerCertificate=True;"));
            });
        }
    }
}
