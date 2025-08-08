using System;
using GradeApp.Data;
using System.Threading.Tasks;

namespace GradeApp.Database.Seeders
{
    public class DatabaseSeeder : BaseSeeder
    {
        public override async Task SeedAsync(GradeAppContext context)
        {
            WriteInfo("Starting database seeding...");

            try
            {
                // Run seeders in order (belangrijk voor foreign keys!)
                await new CourseSeeder().SeedAsync(context);
                await new AssignmentGroupSeeder().SeedAsync(context);
                await new AssignmentSeeder().SeedAsync(context);
                await new TestConfigurationSeeder().SeedAsync(context);
                await new TestResultSeeder().SeedAsync(context);

                WriteSuccess("Database seeding completed successfully! 🎉");
            }
            catch (Exception ex)
            {
                WriteError($"Database seeding failed: {ex.Message}");
                throw;
            }
        }
    }
}