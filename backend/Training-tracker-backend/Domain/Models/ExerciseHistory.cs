using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ExerciseHistory
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty; //ska vara exersice namnet

        public int WeightKg { get; set; }

        public int Reps { get; set; }

        public int SetNumber { get; set; }
        public DateTime PerformedAt = DateTime.Now;


        public int ExerciseId { get; set; }
        public Exercise? Exercise { get; set; } 

        public int UserId { get; set; }
        public User? User { get; set; } 
    }
}
