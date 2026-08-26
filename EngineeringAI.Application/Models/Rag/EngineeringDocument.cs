using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Models.Rag {
    public class EngineeringDocument {
        public string DocumentId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string? EquipmentNumber { get; set; }
        public int? ProjectId { get; set; }
        public string? Supplier { get; set; }
        public string SourceFile { get; set; } = string.Empty;
    }
}
