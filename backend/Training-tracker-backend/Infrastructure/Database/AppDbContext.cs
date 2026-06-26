using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<ExerciseHistory> ExerciseHistories { get; set; }
        public DbSet<WorkoutHistory> WorkoutHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<WorkoutExercise>()
            .HasKey(we => we.Id); // Primärnyckel

            modelBuilder.Entity<WorkoutExercise>()
                .Property(we => we.Id)
                .ValueGeneratedOnAdd(); // Auto increment

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutExercise>()
                .HasIndex(we => new { we.WorkoutId, we.ExerciseId })
                .IsUnique();


            modelBuilder.Entity<Workout>()
                .Property(w => w.UserId)
                .ValueGeneratedNever();


            modelBuilder.Entity<Workout>()
                 .HasOne(w => w.User)
                 .WithMany(u => u.Workouts)
                 .HasForeignKey(w => w.UserId)
                 .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<ExerciseHistory>()
                .HasOne(eh => eh.Exercise)
                .WithMany(e => e.ExerciseHistories)
                .HasForeignKey(eh => eh.ExerciseId);

            modelBuilder.Entity<ExerciseHistory>()
              .HasOne(eh => eh.User)
              .WithMany(u => u.ExerciseHistories)
              .HasForeignKey(eh => eh.UserId);

            modelBuilder.Entity<ExerciseHistory>()
              .HasOne(eh => eh.WorkoutExercise)
              .WithMany(we => we.ExerciseHistories)
              .HasForeignKey(eh => eh.WorkoutExerciseId)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExerciseHistory>()
              .HasOne(eh => eh.WorkoutHistory)
              .WithMany(wh => wh.ExerciseHistories)
              .HasForeignKey(eh => eh.WorkoutHistoryId)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkoutHistory>()
              .HasOne(wh => wh.Workout)
              .WithMany(w => w.WorkoutHistories)
              .HasForeignKey(wh => wh.WorkoutId)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkoutHistory>()
              .HasOne(wh => wh.User)
              .WithMany(u => u.WorkoutHistories)
              .HasForeignKey(wh => wh.UserId)
              .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
