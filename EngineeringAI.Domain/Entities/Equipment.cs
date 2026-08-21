namespace EngineeringAI.Domain.Entities {
    public class Equipment {
        public int Id { get; set; }

        public string EquipmentNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int ProjectId { get; set; }

        public int? SupplierId { get; set; }

        public string? Manufacturer { get; set; }

        public string? EquipmentType { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Project Project { get; set; } = null!;

        public Supplier? Supplier { get; set; }

        public ICollection<Deliverable> Deliverables { get; set; }
            = new List<Deliverable>();
    }
}