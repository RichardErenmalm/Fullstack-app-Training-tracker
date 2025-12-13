using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Workout.Commands.DeleteWorkout
{
    public class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand, bool>
    {
        private readonly IWorkoutRepository _workoutRepository;

        public DeleteWorkoutCommandHandler(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<bool> Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = await _workoutRepository.GetWorkoutByIdAsync(request.Id);

            if (workout == null)
            {
                return false;
            }

            await _workoutRepository.DeleteWorkoutAsync(workout);
            return true;
        }
    }

}
