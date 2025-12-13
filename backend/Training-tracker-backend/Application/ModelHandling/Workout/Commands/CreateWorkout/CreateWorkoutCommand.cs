using Application.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Workout.Commands.CreateWorkout
{
    public class CreateWorkoutCommand : IRequest<OperationResult<WorkoutDto>>
    {
        public string Name { get; set; } = string.Empty;

        public int UserId { get; set; }
    }
}
