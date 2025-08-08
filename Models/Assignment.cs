using System;
using System.Collections.Generic;

namespace GradeApp.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // bijv. "Variabelen Oefening"
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public double Points { get; set; } // Maximaal aantal punten
        public int CanvasId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign Keys - assignment hoort bij course EN assignment group
        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

        public int AssignmentGroupId { get; set; }
        public virtual AssignmentGroup AssignmentGroup { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<TestConfiguration> TestConfigurations { get; set; } = new List<TestConfiguration>();
        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}