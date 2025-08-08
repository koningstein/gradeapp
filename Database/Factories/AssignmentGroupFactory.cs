using GradeApp.Models;
using System;

namespace GradeApp.Database.Factories
{
    public class AssignmentFactory : BaseFactory<Assignment>
    {
        public override Assignment Create()
        {
            return new Assignment
            {
                Name = $"Assignment {Random.Next(1, 100)}",
                Description = $"Description {Random.Next(1, 100)}",
                DueDate = DateTime.Now.AddDays(Random.Next(1, 30)),
                Points = Random.Next(10, 100),
                CanvasId = Random.Next(80000, 99999),
                CourseId = 1, // Default
                AssignmentGroupId = 1, // Default
                CreatedAt = DateTime.Now.AddDays(-Random.Next(1, 50))
            };
        }
    }
}