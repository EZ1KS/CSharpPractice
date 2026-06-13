using Microsoft.EntityFrameworkCore;
using WorkoutTracker.API.Models;

namespace WorkoutTracker.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<WorkoutProgram> WorkoutPrograms { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkoutProgram>()
                .HasIndex(wp => wp.Name)
                .IsUnique();

            modelBuilder.Entity<Exercise>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.WorkoutProgram)
                .WithMany(wp => wp.Exercises)
                .HasForeignKey(e => e.WorkoutProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Exercise)
                .WithMany(e => e.Activities)
                .HasForeignKey(a => a.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Activity>()
                .Property(a => a.Date)
                .HasColumnType("date");
        }
    }
}