using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User {  get; set; } = new User();


        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

    }
}
