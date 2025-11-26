using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ExersiceHistory
    {
        public int Id { get; set; }

        public int ExersiceId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }//ska vara exersice namnet

        public int WeightKg { get; set; }

        public int Reps { get; set; }

        public int SetNumber { get; set; }
    }
}
