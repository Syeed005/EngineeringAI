using Azure.Messaging.ServiceBus;
using EngineeringAI.Domain.Entities;
using EngineeringAI.Functions.Models;
using EngineeringAI.Infrastructure.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EngineeringAI.Functions;

public class EquipmentEventProcessor
{
    private readonly ILogger<EquipmentEventProcessor> _logger;
    private readonly EngineeringDbContext _dbContext;

    public EquipmentEventProcessor(ILogger<EquipmentEventProcessor> logger, EngineeringDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [Function(nameof(EquipmentEventProcessor))]
    public async Task Run([ServiceBusTrigger("equipment-events", Connection = "ServiceBusConnection", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Received Service Bus message {MessageId}.", message.MessageId);

        var equipmentEvent = JsonSerializer.Deserialize<EquipmentCreatedEvent>(
            message.Body.ToString(),
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });

        if (equipmentEvent is null) {
            throw new InvalidOperationException("Unable to deserialize EquipmentCreated event.");
        }

        var alreadyProcessed = await _dbContext.IntegrationAudits.AnyAsync(x => x.EventId == message.MessageId);

        if (alreadyProcessed) {
            _logger.LogWarning("Service Bus message {MessageId} has already been processed.",message.MessageId);

            await messageActions.CompleteMessageAsync(message);
            return;
        }

        var audit = new IntegrationAudit {
            EventId = message.MessageId,
            EventType = equipmentEvent.EventType,
            EquipmentId = equipmentEvent.EquipmentId,
            EquipmentNumber = equipmentEvent.EquipmentNumber,
            Status = "Processed",
            ProcessedAtUtc = DateTime.UtcNow
        };

        _dbContext.IntegrationAudits.Add(audit);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation(
            "Equipment event {MessageId} for equipment {EquipmentNumber} processed successfully.",
            message.MessageId,
            equipmentEvent.EquipmentNumber);

        await messageActions.CompleteMessageAsync(message);
    }
}