using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.DTOs.Equipment {
    public class UpdateEquipmentRequest {
        public string EquipmentNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int ProjectId { get; set; }

        public int? SupplierId { get; set; }

        public string? Manufacturer { get; set; }

        public string? EquipmentType { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
