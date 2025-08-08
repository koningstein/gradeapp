using GradeApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GradeApp.Data
{
    public class GradeAppContext : DbContext
    {
        // DbSets - je "tabellen"
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<AssignmentGroup> AssignmentGroups { get; set; } = null!;
        public DbSet<Assignment> Assignments { get; set; } = null!;
        public DbSet<TestConfiguration> TestConfigurations { get; set; } = null!;
        public DbSet<TestResult> TestResults { get; set; } = null!;

        public GradeAppContext(DbContextOptions<GradeAppContext> options) : base(options) { }
        public GradeAppContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string dbPath = Path.Combine(appDataPath, "GradeApp", "gradeapp.db");

                var directory = Path.GetDirectoryName(dbPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.LogTo(Console.WriteLine);
            }
        }

        // Geen OnModelCreating meer nodig! Models configureren zichzelf 🎉

        // Timestamps (zoals Laravel)
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var createdAtProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "CreatedAt");
                var updatedAtProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "UpdatedAt");

                if (entry.State == EntityState.Added && createdAtProperty != null)
                {
                    if (createdAtProperty.CurrentValue == null ||
                        (createdAtProperty.CurrentValue is DateTime dt && dt == DateTime.MinValue))
                    {
                        createdAtProperty.CurrentValue = DateTime.Now;
                    }
                }

                if (entry.State == EntityState.Modified && updatedAtProperty != null)
                {
                    updatedAtProperty.CurrentValue = DateTime.Now;
                }
            }
        }
    }
}