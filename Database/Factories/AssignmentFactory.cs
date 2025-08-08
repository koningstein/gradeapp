using GradeApp.Models;
using System;

namespace GradeApp.Database.Factories
{
    public class AssignmentGroupFactory : BaseFactory<AssignmentGroup>
    {
        public override AssignmentGroup Create()
        {
            return new AssignmentGroup
            {
                Name = $"Group {Random.Next(1, 20)}",
                Description = $"Description {Random.Next(1, 100)}",
                Position = Random.Next(1, 10),
                GroupWeight = Math.Round(Random.NextDouble(), 2),
                CanvasId = Random.Next(50000, 99999),
                CourseId = 1, // Default
                CreatedAt = DateTime.Now.AddDays(-Random.Next(1, 100))
            };
        }
    }
}