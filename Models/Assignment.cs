using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GradeApp.Models
{
    [Index(nameof(CanvasId), IsUnique = true)]
    public class Assignment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty; // bijv. "Variabelen Oefening"

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public double Points { get; set; } // Maximaal aantal punten

        public int CanvasId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Foreign Keys - assignment hoort bij course EN assignment group
        [Required]
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        [Required]
        public int AssignmentGroupId { get; set; }

        [ForeignKey(nameof(AssignmentGroupId))]
        public virtual AssignmentGroup AssignmentGroup { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<TestConfiguration> TestConfigurations { get; set; } = new List<TestConfiguration>();
        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}