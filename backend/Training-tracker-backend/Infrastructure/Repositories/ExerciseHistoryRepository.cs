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
    public class ExerciseHistoryRepository : IExerciseHistoryRepository
    {
        private readonly AppDbContext _context;
        public ExerciseHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExerciseHistory>> GetAllExerciseHistoriesAsync()
        {
            return await _context.ExerciseHistories.ToListAsync();
        }

        public async Task<ExerciseHistory> GetExerciseHistoryByIdAsync(int id)
        {
            return await _context.ExerciseHistories.FindAsync(id);
        }

        public async Task<int> AddExerciseHistoryAsync(ExerciseHistory exerciseHistory)
        {
            _context.ExerciseHistories.Add(exerciseHistory);
            await _context.SaveChangesAsync();
            return exerciseHistory.Id;
        }

        public async Task UpdateExerciseHistoryAsync(ExerciseHistory exerciseHistory)
        {
            _context.ExerciseHistories.Update(exerciseHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExerciseHistoryAsync(ExerciseHistory exerciseHistory)
        {
            _context.ExerciseHistories.Remove(exerciseHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DetachFromWorkoutExercisesAsync(IEnumerable<int> workoutExerciseIds)
        {
            var entries = await _context.ExerciseHistories
                .Where(eh => eh.WorkoutExerciseId.HasValue && workoutExerciseIds.Contains(eh.WorkoutExerciseId.Value))
                .ToListAsync();
            foreach (var e in entries) e.WorkoutExerciseId = null;
            await _context.SaveChangesAsync();
        }

        public async Task DetachFromWorkoutHistoriesAsync(IEnumerable<int> workoutHistoryIds)
        {
            var entries = await _context.ExerciseHistories
                .Where(eh => eh.WorkoutHistoryId.HasValue && workoutHistoryIds.Contains(eh.WorkoutHistoryId.Value))
                .ToListAsync();
            foreach (var e in entries) e.WorkoutHistoryId = null;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int workoutExerciseId, int setNumber, int workoutHistoryId)
        {
            return await _context.ExerciseHistories
                .AnyAsync(eh => eh.WorkoutExerciseId.HasValue
                    && eh.WorkoutExerciseId.Value == workoutExerciseId
                    && eh.SetNumber == setNumber
                    && eh.WorkoutHistoryId.HasValue
                    && eh.WorkoutHistoryId.Value == workoutHistoryId);
        }
    }
}
