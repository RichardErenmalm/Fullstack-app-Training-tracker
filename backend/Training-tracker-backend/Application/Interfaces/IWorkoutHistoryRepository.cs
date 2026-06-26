using Domain.Models;

namespace Application.Interfaces
{
    public interface IWorkoutHistoryRepository
    {
        public Task<List<WorkoutHistory>> GetAllWorkoutHistoriesAsync();
        public Task<WorkoutHistory> GetWorkoutHistoryByIdAsync(int id);
        public Task<int> AddWorkoutHistoryAsync(WorkoutHistory workoutHistory);
        public Task UpdateWorkoutHistoryAsync(WorkoutHistory workoutHistory);
        public Task DeleteWorkoutHistoryAsync(WorkoutHistory workoutHistory);
        public Task<bool> ExistsAsync(int id);
        public Task DetachFromWorkoutAsync(int workoutId);
    }
}
