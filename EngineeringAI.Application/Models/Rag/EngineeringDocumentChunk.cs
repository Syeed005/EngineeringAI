using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Models.Rag {
    public class EngineeringDocumentChunk {
        public string Id { get; set; } = string.Empty;
        public string DocumentId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string? EquipmentNumber { get; set; }
        public int? ProjectId { get; set; }
        public string? Supplier { get; set; }
        public string SourceFile { get; set; } = string.Empty;
        public int ChunkNumber { get; set; }
        public IReadOnlyList<float> ContentVector { get; set; } = [];
    }
}