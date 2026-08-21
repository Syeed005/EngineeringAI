using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Domain.Entities {
    public class Project {
        public int Id { get; set; }

        public string ProjectNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Customer { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public ICollection<Equipment> Equipment { get; set; }
            = new List<Equipment>();

        public ICollection<Deliverable> Deliverables { get; set; }
            = new List<Deliverable>();
    }
}
