using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GradeApp.Models
{
    [Index(nameof(CanvasId), IsUnique = true)]
    public class AssignmentGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty; // bijv. "Week 1 Opdrachten", "Projecten", "Examens"

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int Position { get; set; } // Volgorde in Canvas (1, 2, 3...)

        [Required]
        [Column(TypeName = "decimal(3,2)")]
        public double GroupWeight { get; set; } // Gewicht voor cijferberekening (0.0 - 1.0)

        public int CanvasId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Foreign Key - bij welke course hoort deze group
        [Required]
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        // Navigation property - een group heeft meerdere assignments
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}