using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.WorkoutExercise.Commands.DeleteWorkoutExercise
{
    public class DeleteWorkoutExerciseCommandHandler
    : IRequestHandler<DeleteWorkoutExerciseCommand, bool>
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;

        public DeleteWorkoutExerciseCommandHandler(IWorkoutExerciseRepository workoutExerciseRepository)
        {
            _workoutExerciseRepository = workoutExerciseRepository;
        }

        public async Task<bool> Handle(DeleteWorkoutExerciseCommand request, CancellationToken cancellationToken)
        {
            var workoutExercise = await _workoutExerciseRepository.GetWorkoutExerciseByIdAsync(request.Id);

            if (workoutExercise == null)
            {
                return false;
            }

            await _workoutExerciseRepository.DeleteWorkoutExerciseAsync(workoutExercise);
            return true;
        }
    }

}
