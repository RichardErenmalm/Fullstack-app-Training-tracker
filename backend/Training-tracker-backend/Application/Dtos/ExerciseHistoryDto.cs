using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ExerciseHistoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty; //ska vara exersice namnet

        public int WeightKg { get; set; }

        public int Reps { get; set; }

        public int SetNumber { get; set; }

        public DateTime DateTime { get; set; }

        public int UserId { get; set; }
        public int ExerciseId { get; set; }
    }
}
