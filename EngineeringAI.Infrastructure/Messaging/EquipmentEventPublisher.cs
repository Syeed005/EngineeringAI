using Azure.Messaging.ServiceBus;
using EngineeringAI.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace EngineeringAI.Infrastructure.Messaging {
    public class EquipmentEventPublisher : IEquipmentEventPublisher {
        private readonly ServiceBusSender _sender;

        public EquipmentEventPublisher(ServiceBusSender sender) {
            _sender = sender;
        }

        public async Task PublishEquipmentCreatedAsync(int equipmentId, string equipmentNumber, int projectId, CancellationToken cancellationToken = default) {
            //var sender = _serviceBusClient.CreateSender("equipment-events");

            var eventData = new {
                EventType = "EquipmentCreated",
                EquipmentId = equipmentId,
                EquipmentNumber = equipmentNumber,
                ProjectId = projectId,
                OccurredAtUtc = DateTime.UtcNow
            };

            var messageBody = JsonSerializer.Serialize(eventData);

            var message = new ServiceBusMessage(messageBody) {
                ContentType = "application/json",
                Subject = "EquipmentCreated",
                MessageId = Guid.NewGuid().ToString()
            };

            await _sender.SendMessageAsync(message, cancellationToken);
        }
    }
}
