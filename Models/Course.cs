using System;
using System.Collections.Generic;

namespace GradeApp.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // bijv. "Programmeren 1"
        public string Code { get; set; } = string.Empty; // bijv. "PROG1"
        public int CanvasId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties - een course heeft meerdere assignment groups
        public virtual ICollection<AssignmentGroup> AssignmentGroups { get; set; } = new List<AssignmentGroup>();

        // Direct toegang tot alle assignments (handig voor overzichten)
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}