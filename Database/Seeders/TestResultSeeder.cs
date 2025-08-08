using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GradeApp.Data;
using GradeApp.Models;
using GradeApp.Database.Factories;
using Microsoft.EntityFrameworkCore;

namespace GradeApp.Database.Seeders
{
    public class TestResultSeeder : BaseSeeder
    {
        public override async Task SeedAsync(GradeAppContext context)
        {
            if (await context.TestResults.AnyAsync())
            {
                WriteWarning("TestResults table already has data, skipping...");
                return;
            }

            WriteInfo("Seeding test results...");

            var testConfigs = await context.TestConfigurations
                .Include(tc => tc.Assignment)
                .ToListAsync();

            var testResults = new List<TestResult>();
            var factory = new TestResultFactory();

            var studentNames = new[]
            {
                "Jan de Vries", "Lisa van Berg", "Ahmed Hassan", "Emma Jansen",
                "Tom Bakker", "Sofia Rodriguez", "David Kim", "Anna Kowalski",
                "Marcus Johnson", "Fatima Al-Zahra", "Piet Janssen", "Maria Garcia"
            };

            foreach (var testConfig in testConfigs)
            {
                // 3-5 test results per configuration (verschillende studenten)
                var resultCount = new Random().Next(3, 6);
                var usedStudents = new List<string>();

                for (int i = 0; i < resultCount; i++)
                {
                    // Kies een student die nog niet gebruikt is voor deze assignment
                    var availableStudents = studentNames.Except(usedStudents).ToList();
                    if (!availableStudents.Any()) break;

                    var studentName = availableStudents[new Random().Next(availableStudents.Count)];
                    usedStudents.Add(studentName);

                    var testResult = factory.Create(tr =>
                    {
                        tr.StudentName = studentName;
                        tr.StudentSubmissionId = $"SUB_{testConfig.AssignmentId}_{i + 1:000}";
                        tr.RepositoryUrl = GenerateRepoUrl(studentName, testConfig.Assignment.Name);
                        tr.AssignmentId = testConfig.AssignmentId;
                        tr.TestConfigurationId = testConfig.Id;
                        tr.ExecutedAt = testConfig.CreatedAt.AddDays(new Random().Next(1, 14));

                        // Varieer de resultaten
                        var success = new Random().NextDouble() > 0.3; // 70% kans op succes
                        if (success)
                        {
                            MakeSuccessfulResult(tr);
                        }
                        else
                        {
                            MakeFailedResult(tr);
                        }
                    });

                    testResults.Add(testResult);
                }
            }

            context.TestResults.AddRange(testResults);
            await context.SaveChangesAsync();

            WriteSuccess($"Created {testResults.Count} test results");
        }

        private string GenerateRepoUrl(string studentName, string assignmentName)
        {
            var cleanStudentName = studentName.Replace(" ", "").ToLower();
            var cleanAssignmentName = assignmentName.Replace(" ", "-").ToLower();
            return $"https://github.com/{cleanStudentName}/{cleanAssignmentName}";
        }

        private void MakeSuccessfulResult(TestResult result)
        {
            var totalTests = new Random().Next(8, 15);
            result.TotalTestsCount = totalTests;
            result.PassedTestsCount = totalTests;
            result.TestsPassed = true;
            result.Score = result.MaxScore;
            result.Status = "Completed";
            result.Feedback = "🎉 Alle tests geslaagd! Uitstekend werk.";
            result.TestOutput = $@"Running test suite...

✅ All {totalTests} tests passed!

Test Summary:
- Basic functionality: ✅ Passed
- Edge cases: ✅ Passed  
- Error handling: ✅ Passed

Duration: {new Random().Next(2, 8)} seconds
Result: SUCCESS 🎉";
        }

        private void MakeFailedResult(TestResult result)
        {
            var totalTests = new Random().Next(8, 15);
            var passedTests = new Random().Next(1, totalTests - 1);
            var failedTests = totalTests - passedTests;

            result.TotalTestsCount = totalTests;
            result.PassedTestsCount = passedTests;
            result.TestsPassed = false;
            result.Score = Math.Round(result.MaxScore * (passedTests / (double)totalTests), 1);
            result.Status = "Completed";
            result.Feedback = $@"⚠️ {passedTests}/{totalTests} tests geslaagd.

Verbeterpunten:
- Check je variabele declaraties
- Vergeet niet null checks toe te voegen
- Test je edge cases grondig

Tip: Run de tests lokaal om de exacte error messages te zien!";

            result.TestOutput = $@"Running test suite...

✅ {passedTests} tests passed
❌ {failedTests} tests failed

Failed tests:
- AssertionError: Expected 5 but got 4
- ValidationError: Input validation failed

Duration: {new Random().Next(3, 12)} seconds
Result: FAILED ❌";
        }
    }
}