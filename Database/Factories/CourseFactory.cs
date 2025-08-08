using GradeApp.Models;
using System;

namespace GradeApp.Database.Factories
{
    public class CourseFactory : BaseFactory<Course>
    {
        public override Course Create()
        {
            return new Course
            {
                Name = $"Course {Random.Next(1, 100)}",
                Code = $"C{Random.Next(100, 999)}",
                CanvasId = Random.Next(10000, 99999),
                CreatedAt = DateTime.Now.AddDays(-Random.Next(1, 365))
            };
        }
    }
}