using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Domain.Entities {
    public class IntegrationAudit {

        public int Id { get; set; }

        public string EventId { get; set; } = string.Empty;

        public string EventType { get; set; } = string.Empty;

        public int? EquipmentId { get; set; }

        public string? EquipmentNumber { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime ProcessedAtUtc { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
