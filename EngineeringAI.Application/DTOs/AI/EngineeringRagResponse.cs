using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.DTOs.AI {
    public class EngineeringRagResponse {
        public string Answer { get; set; } = string.Empty;
        public List<EngineeringRagSource> Sources { get; set; } = [];
    }
}
