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
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly AppDbContext _context;
        public WorkoutRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Workout>> GetAllWorkoutsAsync()
        {
            return await _context.Workouts.ToListAsync();
        }

        public async Task<Workout> GetWorkoutByIdAsync(int id)
        {
            return await _context.Workouts.FindAsync(id);
        }

        public async Task<int> AddWorkoutAsync(Workout workout)
        {
            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();
            return workout.Id;
        }

        public async Task UpdateWorkoutAsync(Workout workout)
        {
            _context.Workouts.Update(workout);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkoutAsync(Workout workout)
        {
            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }
    }

}
