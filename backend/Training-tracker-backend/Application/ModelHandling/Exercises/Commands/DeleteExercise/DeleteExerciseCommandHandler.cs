using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Exercises.Commands.DeleteExercise
{
    public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, bool>
    {

        private readonly IExerciseRepository _exerciseRepository;

        public DeleteExerciseCommandHandler(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<bool> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(request.Id);
            if (exercise == null)
            {
                return false;
            }


            await _exerciseRepository.DeleteExerciseAsync(exercise);
            return true;
        }
    }
}
