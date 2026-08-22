using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Interfaces.Messaging {
    public interface IEquipmentEventPublisher {
        Task PublishEquipmentCreatedAsync(int equipmentId, string equipmentNumber, int projectId, CancellationToken cancellationToken = default);
    }
}
