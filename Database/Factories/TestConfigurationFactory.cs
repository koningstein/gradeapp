using GradeApp.Models;
using System;

namespace GradeApp.Database.Factories
{
    public class TestConfigurationFactory : BaseFactory<TestConfiguration>
    {
        public override TestConfiguration Create()
        {
            return new TestConfiguration
            {
                TestCode = "// Test code",
                TestingInstructions = "Run tests",
                Language = "PHP",
                Framework = "PHPUnit",
                IsActive = true,
                AssignmentId = 1, // Default
                CreatedAt = DateTime.Now.AddDays(-Random.Next(1, 30))
            };
        }
    }
}