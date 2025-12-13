using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface  IExerciseRepository
    {
        public Task<List<Exercise>> GetAllExercisesAsync();
        public Task<Exercise> GetExerciseByIdAsync(int id);
        public Task<int> AddExerciseAsync(Exercise exercise);
        public Task UpdateExerciseAsync(Exercise exercise);

        public Task DeleteExerciseAsync(Exercise exercise);
        Task<bool> ExistsAsync(int entityId);
    }
}
