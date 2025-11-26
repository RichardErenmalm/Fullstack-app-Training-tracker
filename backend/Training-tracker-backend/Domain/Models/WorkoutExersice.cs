using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class WorkoutExersice
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExersiceId { get; set; }
        public int Sets { get; set; }
    }
}
