using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExerciseHistoryRepository
    {
        public Task<List<ExerciseHistory>> GetAllExerciseHistoriesAsync();
        public Task<ExerciseHistory> GetExerciseHistoryByIdAsync(int id);
        public Task<int> AddExerciseHistoryAsync(ExerciseHistory exerciseHistory);
        public Task UpdateExerciseHistoryAsync(ExerciseHistory exerciseHistory);

        public Task DeleteExerciseHistoryAsync(ExerciseHistory exerciseHistory);
    }
}
