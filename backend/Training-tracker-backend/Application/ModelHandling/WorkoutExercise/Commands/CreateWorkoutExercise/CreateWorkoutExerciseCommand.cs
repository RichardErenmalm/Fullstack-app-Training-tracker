using Application.Dtos;
using Domain.Common;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.WorkoutExercise.Commands.CreateWorkoutExercise
{
    public class CreateWorkoutExerciseCommand : IRequest<OperationResult<WorkoutExerciseDto>>
    {
        public int Sets { get; set; }

        public int WorkoutId { get; set; }

        public int ExerciseId { get; set; }
    }
}
