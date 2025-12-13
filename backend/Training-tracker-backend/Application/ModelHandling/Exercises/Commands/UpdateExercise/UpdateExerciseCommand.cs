using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Exercises.Commands.UpdateExercise
{
    public class UpdateExerciseCommand : IRequest<ExerciseDto>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
