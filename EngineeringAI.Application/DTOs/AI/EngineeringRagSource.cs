using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.DTOs.AI {
    public class EngineeringRagSource {
        public string SourceFile { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string DocumentId { get; set; } = string.Empty;
        public int ChunkNumber { get; set; }
        public double Score { get; set; }
    }
}
