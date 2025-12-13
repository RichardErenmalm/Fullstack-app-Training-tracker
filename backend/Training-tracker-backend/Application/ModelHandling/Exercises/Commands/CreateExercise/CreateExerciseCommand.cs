using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Exercises.Commands.CreateExercise
{
    public class CreateExerciseCommand : IRequest<ExerciseDto>
    {
        public required string Name { get; set; }
    }
}
