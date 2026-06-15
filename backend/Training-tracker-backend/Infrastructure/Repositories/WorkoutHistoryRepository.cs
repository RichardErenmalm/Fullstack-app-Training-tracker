using Application.Interfaces;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkoutHistoryRepository : IWorkoutHistoryRepository
    {
        private readonly AppDbContext _context;

        public WorkoutHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkoutHistory>> GetAllWorkoutHistoriesAsync()
        {
            return await _context.WorkoutHistories.ToListAsync();
        }

        public async Task<WorkoutHistory> GetWorkoutHistoryByIdAsync(int id)
        {
            return await _context.WorkoutHistories.FindAsync(id);
        }

        public async Task<int> AddWorkoutHistoryAsync(WorkoutHistory workoutHistory)
        {
            _context.WorkoutHistories.Add(workoutHistory);
            await _context.SaveChangesAsync();
            return workoutHistory.Id;
        }

        public async Task UpdateWorkoutHistoryAsync(WorkoutHistory workoutHistory)
        {
            _context.WorkoutHistories.Update(workoutHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkoutHistoryAsync(WorkoutHistory workoutHistory)
        {
            _context.WorkoutHistories.Remove(workoutHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.WorkoutHistories.AnyAsync(wh => wh.Id == id);
        }

        public async Task DetachFromWorkoutAsync(int workoutId)
        {
            var entries = await _context.WorkoutHistories
                .Where(wh => wh.WorkoutId == workoutId)
                .ToListAsync();
            foreach (var e in entries) e.WorkoutId = null;
            await _context.SaveChangesAsync();
        }
    }
}
