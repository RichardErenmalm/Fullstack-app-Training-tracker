using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class WorkoutExerciseDto
    {
        public int Id { get; set; }

        public int Sets { get; set; }

        public int WorkoutId { get; set; }

        public int ExerciseId { get; set; }
    }
}
