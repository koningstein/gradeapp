using GradeApp.Data;
using GradeApp.Database.Factories;
using GradeApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GradeApp.Database.Seeders
{
    public class CourseSeeder : BaseSeeder
    {
        public override async Task SeedAsync(GradeAppContext context)
        {
            if (await context.Courses.AnyAsync())
            {
                WriteWarning("Courses table already has data, skipping...");
                return;
            }

            WriteInfo("Seeding courses...");

            // Specifieke MBO Software Developer courses
            var courses = new List<Course>
            {
                new Course
                {
                    Name = "Programmeren 1",
                    Code = "PROG1",
                    CanvasId = 12345,
                    CreatedAt = DateTime.Now.AddDays(-180)
                },
                new Course
                {
                    Name = "Web Development",
                    Code = "WEB1",
                    CanvasId = 12346,
                    CreatedAt = DateTime.Now.AddDays(-150)
                },
                new Course
                {
                    Name = "Database Design",
                    Code = "DB1",
                    CanvasId = 12347,
                    CreatedAt = DateTime.Now.AddDays(-120)
                },
                new Course
                {
                    Name = "Software Engineering",
                    Code = "SE1",
                    CanvasId = 12348,
                    CreatedAt = DateTime.Now.AddDays(-90)
                }
            };

            // Voeg specifieke courses toe
            context.Courses.AddRange(courses);

            // Voeg ook wat random courses toe via factory
            var factory = new CourseFactory();
            var randomCourses = factory.Create(3);
            context.Courses.AddRange(randomCourses);

            await context.SaveChangesAsync();

            WriteSuccess($"Created {courses.Count + randomCourses.Count} courses");
        }
    }
}