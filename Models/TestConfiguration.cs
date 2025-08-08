using System;
using System.Collections.Generic;

namespace GradeApp.Models
{
    public class TestConfiguration
    {
        public int Id { get; set; }
        public string TestCode { get; set; } = string.Empty; // De daadwerkelijke test code
        public string TestingInstructions { get; set; } = string.Empty; // Hoe tests uitvoeren
        public string Language { get; set; } = string.Empty; // bijv. "PHP", "C#", "Python"
        public string Framework { get; set; } = string.Empty; // bijv. "PHPUnit", "Pest", "NUnit"
        public bool IsActive { get; set; } = true; // Of deze test actief is
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } // Laatste update (nullable)

        // Foreign Key - bij welk assignment hoort deze test
        public int AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; } = null!;

        // Navigation property - een test config kan meerdere resultaten hebben
        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}