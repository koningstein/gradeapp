using GradeApp.Models;
using System;

namespace GradeApp.Database.Factories
{
    public class TestResultFactory : BaseFactory<TestResult>
    {
        public override TestResult Create()
        {
            var totalTests = Random.Next(5, 15);
            var passedTests = Random.Next(0, totalTests + 1);

            return new TestResult
            {
                StudentSubmissionId = Random.Next(100000, 999999).ToString(),
                StudentName = $"Student {Random.Next(1, 100)}",
                RepositoryUrl = $"https://github.com/student{Random.Next(1, 100)}/repo{Random.Next(1, 50)}",
                Score = Random.Next(0, 100),
                MaxScore = 100,
                Feedback = "Test feedback",
                TestOutput = $"{passedTests}/{totalTests} tests passed",
                TestsPassed = passedTests == totalTests,
                PassedTestsCount = passedTests,
                TotalTestsCount = totalTests,
                ExecutedAt = DateTime.Now.AddDays(-Random.Next(0, 7)),
                Status = "Completed",
                AssignmentId = 1, // Default
                TestConfigurationId = 1 // Default
            };
        }
    }
}