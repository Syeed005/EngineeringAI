using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Security.KeyVault.Secrets;
using EngineeringAI.Api.ExceptionHandling;
using EngineeringAI.Api.Filters;
using EngineeringAI.Application;
using EngineeringAI.Application.DTOs.Equipment;
using EngineeringAI.Infrastructure;
using EngineeringAI.Infrastructure.Data;
using Scalar.AspNetCore;


public partial class Program {
    private static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
                
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddScoped<ValidationFilter<CreateEquipmentRequest>>();
        builder.Services.AddScoped<ValidationFilter<UpdateEquipmentRequest>>();
        builder.Services.AddScoped<ValidationFilter<EquipmentQueryParameters>>();

        if (!builder.Environment.IsDevelopment()) {
            builder.Services.AddOpenTelemetry().UseAzureMonitor();
        }
        builder.Services.AddHealthChecks().AddDbContextCheck<EngineeringDbContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        //establishes the foundation for standardized errors.
        app.UseExceptionHandler();
        app.UseStatusCodePages();

        app.MapHealthChecks("/health");

        app.MapGet("/test-keyvault", async () =>
        {
            var client = new SecretClient(
                new Uri("https://kv-engineeringai-dev-001.vault.azure.net/"),
                new DefaultAzureCredential());

            var secret = await client.GetSecretAsync("EngineeringAI-TestSecret");

            return Results.Ok(new {
                Message = "Key Vault connection successful",
                SecretValue = secret.Value.Value
            });
        });


        app.Run();
    }
}