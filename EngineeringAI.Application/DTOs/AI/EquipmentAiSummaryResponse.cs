using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.DTOs.AI {
    public class EquipmentAiSummaryResponse {
        public int EquipmentId { get; set; }
        public string Summary { get; set; } = string.Empty;
        public List<string> MissingInformation { get; set; } = [];
        public List<string> Risks { get; set; } = [];
        public List<string> RecommendedActions { get; set; } = [];
    }
}
