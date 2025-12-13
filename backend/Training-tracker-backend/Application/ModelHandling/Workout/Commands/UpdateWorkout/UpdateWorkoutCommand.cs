using Application.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Workout.Commands.UpdateWorkout
{
    public class UpdateWorkoutCommand : IRequest<OperationResult<WorkoutDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int UserId { get; set; }
    }
}
