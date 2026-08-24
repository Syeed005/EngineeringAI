using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Functions.Models {
    public class EquipmentCreatedEvent {
        public string EventType { get; set; } = string.Empty;
        public int EquipmentId { get; set; }
        public string EquipmentNumber { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public DateTime OccurredAtUtc { get; set; }
    }
}
