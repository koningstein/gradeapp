using System;
using System.Collections.Generic;

namespace GradeApp.Models
{
    public class TestResult
    {
        public int Id { get; set; }
        public string StudentSubmissionId { get; set; } = string.Empty; // Canvas submission ID
        public string StudentName { get; set; } = string.Empty;
        public string RepositoryUrl { get; set; } = string.Empty; // GitHub/GitLab URL uit submission
        public double Score { get; set; } // Behaalde score
        public double MaxScore { get; set; } // Maximale score
        public string Feedback { get; set; } = string.Empty; // Feedback voor student
        public string TestOutput { get; set; } = string.Empty; // Ruwe test output
        public bool TestsPassed { get; set; } // Of alle tests geslaagd zijn
        public int PassedTestsCount { get; set; } // Aantal geslaagde tests
        public int TotalTestsCount { get; set; } // Totaal aantal tests
        public DateTime ExecutedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending"; // Pending, Running, Completed, Failed

        // Foreign Keys
        public int AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; } = null!;

        public int TestConfigurationId { get; set; }
        public virtual TestConfiguration TestConfiguration { get; set; } = null!;
    }
}