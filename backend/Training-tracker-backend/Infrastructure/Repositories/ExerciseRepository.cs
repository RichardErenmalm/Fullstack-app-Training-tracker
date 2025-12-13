using Application.ModelHandling.Exercises.Querries.GetAllExercises;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext _context;
        public ExerciseRepository(AppDbContext context) 
        { 
            _context = context;
        }

        public async Task<List<Exercise>> GetAllExercisesAsync()
        {
            return await _context.Exercises.ToListAsync();
        }

        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            return await _context.Exercises.FindAsync(id);
        }

        public async Task<int> AddExerciseAsync(Exercise exercise)
        {
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            return exercise.Id;
        }

        public async Task UpdateExerciseAsync(Exercise exercise)
        {
            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExerciseAsync(Exercise exercise)
        {
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Exercises.AnyAsync(u => u.Id == id);
        }

    }
}
