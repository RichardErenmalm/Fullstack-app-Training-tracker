using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public  string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public ICollection<ExerciseHistory> ExerciseHistories { get; set; } = new List<ExerciseHistory>();
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
        public ICollection<WorkoutHistory> WorkoutHistories { get; set; } = new List<WorkoutHistory>();
    }
}
