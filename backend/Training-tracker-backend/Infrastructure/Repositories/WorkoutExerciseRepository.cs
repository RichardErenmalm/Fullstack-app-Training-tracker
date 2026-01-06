using Application.Interfaces;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class WorkoutExerciseRepository : IWorkoutExerciseRepository
    {
        private readonly AppDbContext _context;
        public WorkoutExerciseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkoutExercise>> GetAllWorkoutExercisesAsync()
        {
            return await _context.WorkoutExercises.ToListAsync();
        }

        public async Task<WorkoutExercise> GetWorkoutExerciseByIdAsync(int id)
        {
            return await _context.WorkoutExercises.FindAsync(id);
        }

        public async Task<int> AddWorkoutExerciseAsync(WorkoutExercise workoutExercise)
        {
            _context.Entry(new Workout { Id = workoutExercise.WorkoutId }).State = EntityState.Unchanged;
            _context.Entry(new Exercise { Id = workoutExercise.ExerciseId }).State = EntityState.Unchanged;

            _context.WorkoutExercises.Add(workoutExercise);
            await _context.SaveChangesAsync();
            return workoutExercise.Id;
        }

        public async Task UpdateWorkoutExerciseAsync(WorkoutExercise workoutExercise)
        {
            _context.WorkoutExercises.Update(workoutExercise);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkoutExerciseAsync(WorkoutExercise workoutExercise)
        {
            _context.WorkoutExercises.Remove(workoutExercise);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExerciseAlreadyAddedToWorkoutAsync(int workoutId, int exerciseId)
        {
            return await _context.WorkoutExercises
                .AnyAsync(x => x.WorkoutId == workoutId && x.ExerciseId == exerciseId);
        }
        public async Task<bool> ExerciseAlreadyAddedToWorkoutUpdateAsync(int workoutId, int exerciseId, int excludeId)
        {
            return await _context.WorkoutExercises
         .AnyAsync(x => x.WorkoutId == workoutId
                     && x.ExerciseId == exerciseId
                     && x.Id != excludeId);
        }
        public async Task<bool> WorkoutExistsAsync(int id)
        {
            return await _context.Workouts.AnyAsync(u => u.Id == id);
        }
        public async Task<bool> ExerciseExistsAsync(int id)
        {
            return await _context.Exercises.AnyAsync(u => u.Id == id);
        }

        public async Task<List<WorkoutExercise>> GetWorkoutExercisesByWorkoutIdAsync(int workoutId)
        {
            return await _context.WorkoutExercises
                .Where(we => we.WorkoutId == workoutId)
                .Include(we => we.Exercise)
                .ToListAsync();
        }
    }

}
