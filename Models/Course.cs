using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GradeApp.Models
{
    [Index(nameof(CanvasId), IsUnique = true)]
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty; // bijv. "Programmeren 1"

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty; // bijv. "PROG1"

        public int CanvasId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties - een course heeft meerdere assignment groups
        public virtual ICollection<AssignmentGroup> AssignmentGroups { get; set; } = new List<AssignmentGroup>();

        // Direct toegang tot alle assignments (handig voor overzichten)
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}