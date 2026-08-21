namespace EngineeringAI.Domain.Entities {
    public class Deliverable {
        public int Id { get; set; }

        public string DocumentNumber { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string? Revision { get; set; }

        public int ProjectId { get; set; }

        public int? EquipmentId { get; set; }

        public string? DocumentType { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? FileName { get; set; }

        public string? FilePath { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Project Project { get; set; } = null!;

        public Equipment? Equipment { get; set; }
    }
}