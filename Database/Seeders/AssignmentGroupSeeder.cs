using GradeApp.Data;
using GradeApp.Database.Factories;
using GradeApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GradeApp.Database.Seeders
{
    public class AssignmentGroupSeeder : BaseSeeder
    {
        public override async Task SeedAsync(GradeAppContext context)
        {
            if (await context.AssignmentGroups.AnyAsync())
            {
                WriteWarning("AssignmentGroups table already has data, skipping...");
                return;
            }

            WriteInfo("Seeding assignment groups...");

            var courses = await context.Courses.ToListAsync();
            var assignmentGroups = new List<AssignmentGroup>();

            foreach (var course in courses)
            {
                // Specifieke groups per course
                var groups = new[]
                {
                    new AssignmentGroup
                    {
                        Name = "Week 1-4 Basis",
                        Description = "Introductie en basis concepten",
                        Position = 1,
                        GroupWeight = 0.20,
                        CanvasId = 50000 + course.Id * 10 + 1,
                        CourseId = course.Id,
                        CreatedAt = course.CreatedAt.AddDays(1)
                    },
                    new AssignmentGroup
                    {
                        Name = "Week 5-8 Gevorderd",
                        Description = "Gevorderde onderwerpen",
                        Position = 2,
                        GroupWeight = 0.30,
                        CanvasId = 50000 + course.Id * 10 + 2,
                        CourseId = course.Id,
                        CreatedAt = course.CreatedAt.AddDays(30)
                    },
                    new AssignmentGroup
                    {
                        Name = "Projecten",
                        Description = "Praktijkgerichte projecten",
                        Position = 3,
                        GroupWeight = 0.40,
                        CanvasId = 50000 + course.Id * 10 + 3,
                        CourseId = course.Id,
                        CreatedAt = course.CreatedAt.AddDays(60)
                    },
                    new AssignmentGroup
                    {
                        Name = "Toetsen",
                        Description = "Examens en toetsen",
                        Position = 4,
                        GroupWeight = 0.10,
                        CanvasId = 50000 + course.Id * 10 + 4,
                        CourseId = course.Id,
                        CreatedAt = course.CreatedAt.AddDays(90)
                    }
                };

                assignmentGroups.AddRange(groups);
            }

            context.AssignmentGroups.AddRange(assignmentGroups);
            await context.SaveChangesAsync();

            WriteSuccess($"Created {assignmentGroups.Count} assignment groups");
        }
    }
}