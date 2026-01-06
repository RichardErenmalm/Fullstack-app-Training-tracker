using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWorkoutExerciseRepository
    {
        public Task<List<WorkoutExercise>> GetAllWorkoutExercisesAsync();
        public Task<WorkoutExercise> GetWorkoutExerciseByIdAsync(int id);
        public Task<int> AddWorkoutExerciseAsync(WorkoutExercise workoutExercise);
        public Task UpdateWorkoutExerciseAsync(WorkoutExercise workoutExercise);

        public Task DeleteWorkoutExerciseAsync(WorkoutExercise workoutExercise);
        public Task<bool> ExerciseAlreadyAddedToWorkoutAsync( int workoutId, int ExerciseId);
        public Task<bool> ExerciseAlreadyAddedToWorkoutUpdateAsync( int workoutId, int ExerciseId, int excludeId);
        Task<bool> WorkoutExistsAsync(int entityId);
        Task<bool> ExerciseExistsAsync(int entityId);

        public Task<List<WorkoutExercise>> GetWorkoutExercisesByWorkoutIdAsync(int workoutId);
    }

}
