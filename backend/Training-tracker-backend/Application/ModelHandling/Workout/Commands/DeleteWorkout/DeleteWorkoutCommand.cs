using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Workout.Commands.DeleteWorkout
{
    public class DeleteWorkoutCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
