using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeApp.Models
{
    public class TestResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string StudentSubmissionId { get; set; } = string.Empty; // Canvas submission ID

        [Required]
        [MaxLength(200)]
        public string StudentName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string RepositoryUrl { get; set; } = string.Empty; // GitHub/GitLab URL uit submission

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public double Score { get; set; } // Behaalde score

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public double MaxScore { get; set; } // Maximale score

        [MaxLength(5000)]
        public string Feedback { get; set; } = string.Empty; // Feedback voor student

        [MaxLength(10000)]
        public string TestOutput { get; set; } = string.Empty; // Ruwe test output

        [Required]
        public bool TestsPassed { get; set; } // Of alle tests geslaagd zijn

        [Required]
        public int PassedTestsCount { get; set; } // Aantal geslaagde tests

        [Required]
        public int TotalTestsCount { get; set; } // Totaal aantal tests

        [Required]
        public DateTime ExecutedAt { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Running, Completed, Failed

        // Foreign Keys
        [Required]
        public int AssignmentId { get; set; }

        [ForeignKey(nameof(AssignmentId))]
        public virtual Assignment Assignment { get; set; } = null!;

        [Required]
        public int TestConfigurationId { get; set; }

        [ForeignKey(nameof(TestConfigurationId))]
        public virtual TestConfiguration TestConfiguration { get; set; } = null!;
    }
}