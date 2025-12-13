using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWorkoutRepository
    {
        public Task<List<Workout>> GetAllWorkoutsAsync();
        public Task<Workout> GetWorkoutByIdAsync(int id);
        public Task<int> AddWorkoutAsync(Workout workout);
        public Task UpdateWorkoutAsync(Workout workout);

        public Task DeleteWorkoutAsync(Workout workout);
        Task<bool> ExistsAsync(int entityId);
    }

}
