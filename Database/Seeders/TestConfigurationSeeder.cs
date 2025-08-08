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
    public class TestConfigurationSeeder : BaseSeeder
    {
        public override async Task SeedAsync(GradeAppContext context)
        {
            if (await context.TestConfigurations.AnyAsync())
            {
                WriteWarning("TestConfigurations table already has data, skipping...");
                return;
            }

            WriteInfo("Seeding test configurations...");

            var assignments = await context.Assignments
                .Include(a => a.Course)
                .ToListAsync();

            var testConfigs = new List<TestConfiguration>();
            var factory = new TestConfigurationFactory();

            // Alleen voor programmeer-gerelateerde assignments
            var programmingAssignments = assignments.Where(a =>
                a.Course.Code.StartsWith("PROG") ||
                a.Course.Code.StartsWith("WEB")).ToList();

            foreach (var assignment in programmingAssignments)
            {
                var language = GetLanguageForCourse(assignment.Course.Code);
                var framework = GetFrameworkForLanguage(language);

                var testConfig = factory.Create(tc =>
                {
                    tc.TestCode = GetTestCodeTemplate(language, framework);
                    tc.TestingInstructions = GetTestInstructions(language, framework);
                    tc.Language = language;
                    tc.Framework = framework;
                    tc.AssignmentId = assignment.Id;
                    tc.CreatedAt = assignment.CreatedAt.AddDays(1);
                });

                testConfigs.Add(testConfig);
            }

            context.TestConfigurations.AddRange(testConfigs);
            await context.SaveChangesAsync();

            WriteSuccess($"Created {testConfigs.Count} test configurations");
        }

        private string GetLanguageForCourse(string courseCode)
        {
            return courseCode switch
            {
                "PROG1" => "PHP",
                "WEB1" => "JavaScript",
                _ => "PHP"
            };
        }

        private string GetFrameworkForLanguage(string language)
        {
            return language switch
            {
                "PHP" => "PHPUnit",
                "JavaScript" => "Jest",
                "C#" => "NUnit",
                _ => "PHPUnit"
            };
        }

        private string GetTestCodeTemplate(string language, string framework)
        {
            return language switch
            {
                "PHP" => @"<?php
use PHPUnit\Framework\TestCase;

class AssignmentTest extends TestCase
{
    public function testBasicFunctionality()
    {
        // TODO: Implementeer test voor basic functionaliteit
        $this->assertTrue(true);
    }

    public function testEdgeCases()
    {
        // TODO: Test edge cases en error handling
        $this->assertNotNull(null);
    }
}",
                "JavaScript" => @"describe('Assignment Tests', () => {
    test('should test basic functionality', () => {
        // TODO: Implementeer test hier
        expect(true).toBe(true);
    });

    test('should handle edge cases', () => {
        // TODO: Test edge cases
        expect(null).toBeNull();
    });
});",
                _ => "// Basic test template\n// TODO: Implement actual tests"
            };
        }

        private string GetTestInstructions(string language, string framework)
        {
            return language switch
            {
                "PHP" => @"1. Clone repository: git clone <repository_url>
2. Install dependencies: composer install
3. Run tests: ./vendor/bin/phpunit
4. Check exit code en parse XML output",
                "JavaScript" => @"1. Clone repository: git clone <repository_url>
2. Install dependencies: npm install
3. Run tests: npm test
4. Parse JSON output voor resultaten",
                _ => "Run tests and check output"
            };
        }
    }
}