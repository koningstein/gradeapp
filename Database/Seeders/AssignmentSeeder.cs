using GradeApp.Data;
using GradeApp.Database.Factories;
using GradeApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GradeApp.Database.Seeders
{
    public class AssignmentSeeder : BaseSeeder
    {
        public override async Task SeedAsync(GradeAppContext context)
        {
            if (await context.Assignments.AnyAsync())
            {
                WriteWarning("Assignments table already has data, skipping...");
                return;
            }

            WriteInfo("Seeding assignments...");

            var assignmentGroups = await context.AssignmentGroups
                .Include(ag => ag.Course)
                .ToListAsync();

            var assignments = new List<Assignment>();
            var factory = new AssignmentFactory();

            foreach (var group in assignmentGroups)
            {
                // 2-3 assignments per group
                var assignmentCount = new Random().Next(2, 4);

                for (int i = 1; i <= assignmentCount; i++)
                {
                    // Gebruik factory voor basis assignment, dan override specifieke data
                    var assignment = factory.Create(a =>
                    {
                        a.Name = $"{group.Course.Code} - {group.Name} - Opdracht {i}";
                        a.Description = GetAssignmentDescription(group.Course.Code, i);
                        a.DueDate = group.CreatedAt.AddDays(7 * i);
                        a.Points = GetPointsForGroup(group.Name);
                        a.CanvasId = 80000 + group.Id * 100 + i;
                        a.CourseId = group.CourseId;
                        a.AssignmentGroupId = group.Id;
                        a.CreatedAt = group.CreatedAt;
                    });

                    assignments.Add(assignment);
                }
            }

            context.Assignments.AddRange(assignments);
            await context.SaveChangesAsync();

            WriteSuccess($"Created {assignments.Count} assignments");
        }

        private string GetAssignmentDescription(string courseCode, int assignmentNumber)
        {
            var descriptions = new Dictionary<string, string[]>
            {
                ["PROG1"] = new[]
                {
                    "Leer werken met variabelen en data types",
                    "Implementeer loops en conditional statements",
                    "Bouw een eenvoudige calculator met functies"
                },
                ["WEB1"] = new[]
                {
                    "Bouw een responsive HTML/CSS website",
                    "Voeg JavaScript interactiviteit toe",
                    "Maak een contact formulier met validatie"
                },
                ["DB1"] = new[]
                {
                    "Ontwerp een database voor een webshop",
                    "Schrijf complexe SQL queries",
                    "Implementeer stored procedures"
                },
                ["SE1"] = new[]
                {
                    "Maak een software ontwerp document",
                    "Implementeer design patterns",
                    "Bouw een volledige applicatie"
                }
            };

            if (descriptions.ContainsKey(courseCode) && assignmentNumber <= descriptions[courseCode].Length)
            {
                return descriptions[courseCode][assignmentNumber - 1];
            }

            return $"Praktijkopdracht {assignmentNumber} voor {courseCode}";
        }

        private double GetPointsForGroup(string groupName)
        {
            return groupName.ToLower() switch
            {
                var name when name.Contains("project") => 100,
                var name when name.Contains("toets") => 50,
                var name when name.Contains("examen") => 100,
                _ => 25
            };
        }
    }
}