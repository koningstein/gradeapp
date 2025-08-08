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

        // Constructor voor dependency injection
        public GradeAppContext(DbContextOptions<GradeAppContext> options) : base(options)
        {
        }

        // Parameterloze constructor voor design-time (migrations)
        public GradeAppContext()
        {
        }

        // OnConfiguring - database verbinding (zoals Laravel database.php)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Database path
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string dbPath = Path.Combine(appDataPath, "GradeApp", "gradeapp.db");

                // Zorg dat directory bestaat
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

                optionsBuilder.UseSqlite($"Data Source={dbPath}");

                // Enable sensitive data logging voor development
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.LogTo(Console.WriteLine);
            }
        }

        // OnModelCreating - tabel configuratie (zoals Laravel migrations)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Course configuratie
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Code).IsRequired().HasMaxLength(50);
                entity.Property(c => c.CreatedAt).IsRequired();
                entity.HasIndex(c => c.CanvasId).IsUnique();
            });

            // AssignmentGroup configuratie
            modelBuilder.Entity<AssignmentGroup>(entity =>
            {
                entity.HasKey(ag => ag.Id);
                entity.Property(ag => ag.Name).IsRequired().HasMaxLength(200);
                entity.Property(ag => ag.Description).HasMaxLength(1000);
                entity.Property(ag => ag.Position).IsRequired();
                entity.Property(ag => ag.GroupWeight).IsRequired().HasPrecision(3, 2);
                entity.Property(ag => ag.CreatedAt).IsRequired();
                entity.HasIndex(ag => ag.CanvasId).IsUnique();

                // Foreign key constraint
                entity.HasOne(ag => ag.Course)
                      .WithMany(c => c.AssignmentGroups)
                      .HasForeignKey(ag => ag.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Assignment configuratie  
            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Name).IsRequired().HasMaxLength(200);
                entity.Property(a => a.Description).HasMaxLength(2000);
                entity.Property(a => a.DueDate).IsRequired();
                entity.Property(a => a.Points).IsRequired().HasPrecision(5, 2);
                entity.Property(a => a.CreatedAt).IsRequired();
                entity.HasIndex(a => a.CanvasId).IsUnique();

                // Foreign key constraints
                entity.HasOne(a => a.Course)
                      .WithMany(c => c.Assignments)
                      .HasForeignKey(a => a.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.AssignmentGroup)
                      .WithMany(ag => ag.Assignments)
                      .HasForeignKey(a => a.AssignmentGroupId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // TestConfiguration configuratie
            modelBuilder.Entity<TestConfiguration>(entity =>
            {
                entity.HasKey(tc => tc.Id);
                entity.Property(tc => tc.TestCode).IsRequired();
                entity.Property(tc => tc.TestingInstructions).IsRequired();
                entity.Property(tc => tc.Language).IsRequired().HasMaxLength(50);
                entity.Property(tc => tc.Framework).IsRequired().HasMaxLength(50);
                entity.Property(tc => tc.IsActive).IsRequired().HasDefaultValue(true);
                entity.Property(tc => tc.CreatedAt).IsRequired();

                // Foreign key constraint
                entity.HasOne(tc => tc.Assignment)
                      .WithMany(a => a.TestConfigurations)
                      .HasForeignKey(tc => tc.AssignmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // TestResult configuratie
            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.HasKey(tr => tr.Id);
                entity.Property(tr => tr.StudentSubmissionId).IsRequired().HasMaxLength(100);
                entity.Property(tr => tr.StudentName).IsRequired().HasMaxLength(200);
                entity.Property(tr => tr.RepositoryUrl).IsRequired().HasMaxLength(500);
                entity.Property(tr => tr.Score).IsRequired().HasPrecision(5, 2);
                entity.Property(tr => tr.MaxScore).IsRequired().HasPrecision(5, 2);
                entity.Property(tr => tr.Feedback).HasMaxLength(5000);
                entity.Property(tr => tr.TestOutput).HasMaxLength(10000);
                entity.Property(tr => tr.TestsPassed).IsRequired();
                entity.Property(tr => tr.PassedTestsCount).IsRequired();
                entity.Property(tr => tr.TotalTestsCount).IsRequired();
                entity.Property(tr => tr.ExecutedAt).IsRequired();
                entity.Property(tr => tr.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");

                // Foreign key constraints
                entity.HasOne(tr => tr.Assignment)
                      .WithMany(a => a.TestResults)
                      .HasForeignKey(tr => tr.AssignmentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tr => tr.TestConfiguration)
                      .WithMany(tc => tc.TestResults)
                      .HasForeignKey(tr => tr.TestConfigurationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        // Override SaveChanges voor automatic timestamps (zoals Laravel)
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
                if (entry.State == EntityState.Added)
                {
                    if (entry.Property("CreatedAt").CurrentValue == null ||
                        (DateTime)entry.Property("CreatedAt").CurrentValue == DateTime.MinValue)
                    {
                        entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                    }
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                }
            }
        }
    }
}