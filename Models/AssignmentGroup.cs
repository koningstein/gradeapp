using System;
using System.Collections.Generic;

namespace GradeApp.Models
{
    public class AssignmentGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // bijv. "Week 1 Opdrachten", "Projecten", "Examens"
        public string? Description { get; set; }
        public int Position { get; set; } // Volgorde in Canvas (1, 2, 3...)
        public double GroupWeight { get; set; } // Gewicht voor cijferberekening (0.0 - 1.0)
        public int CanvasId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Foreign Key - bij welke course hoort deze group
        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

        // Navigation property - een group heeft meerdere assignments
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
