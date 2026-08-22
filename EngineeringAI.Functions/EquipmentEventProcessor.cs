using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EngineeringAI.Functions;

public class EquipmentEventProcessor
{
    private readonly ILogger<EquipmentEventProcessor> _logger;

    public EquipmentEventProcessor(ILogger<EquipmentEventProcessor> logger)
    {
        _logger = logger;
    }

    [Function(nameof(EquipmentEventProcessor))]
    public async Task Run([ServiceBusTrigger("equipment-events", Connection = "ServiceBusConnection", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        //simulating DLQ behaviuor
        if (message.Body.ToString().Contains("EQ-DLQ-001")) {
            throw new InvalidOperationException("Simulated processing failure for dead-letter testing.");
        }

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}