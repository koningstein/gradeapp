using GradeApp.Data;
using System;
using System.Threading.Tasks;

namespace GradeApp.Database.Seeders
{
    public interface ISeeder
    {
        Task SeedAsync(GradeAppContext context);
    }

    public abstract class BaseSeeder : ISeeder
    {
        public abstract Task SeedAsync(GradeAppContext context);

        protected void WriteInfo(string message)
        {
            Console.WriteLine($"[SEEDER] {message}");
        }

        protected void WriteSuccess(string message)
        {
            Console.WriteLine($"[SEEDER] ✅ {message}");
        }

        protected void WriteWarning(string message)
        {
            Console.WriteLine($"[SEEDER] ⚠️ {message}");
        }

        protected void WriteError(string message)
        {
            Console.WriteLine($"[SEEDER] ❌ {message}");
        }
    }
}