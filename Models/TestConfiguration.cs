using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeApp.Models
{
    public class TestConfiguration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TestCode { get; set; } = string.Empty; // De daadwerkelijke test code

        [Required]
        public string TestingInstructions { get; set; } = string.Empty; // Hoe tests uitvoeren

        [Required]
        [MaxLength(50)]
        public string Language { get; set; } = string.Empty; // bijv. "PHP", "C#", "Python"

        [Required]
        [MaxLength(50)]
        public string Framework { get; set; } = string.Empty; // bijv. "PHPUnit", "Pest", "NUnit"

        public bool IsActive { get; set; } = true; // Of deze test actief is

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; } // Laatste update (nullable)

        // Foreign Key - bij welk assignment hoort deze test
        [Required]
        public int AssignmentId { get; set; }

        [ForeignKey(nameof(AssignmentId))]
        public virtual Assignment Assignment { get; set; } = null!;

        // Navigation property - een test config kan meerdere resultaten hebben
        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}