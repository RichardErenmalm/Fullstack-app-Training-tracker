using Application.Interfaces;
using MediatR;

namespace Application.ModelHandling.WorkoutHistory.Commands.DeleteWorkoutHistory
{
    public class DeleteWorkoutHistoryCommandHandler
        : IRequestHandler<DeleteWorkoutHistoryCommand, bool>
    {
        private readonly IWorkoutHistoryRepository _workoutHistoryRepository;

        public DeleteWorkoutHistoryCommandHandler(IWorkoutHistoryRepository workoutHistoryRepository)
        {
            _workoutHistoryRepository = workoutHistoryRepository;
        }

        public async Task<bool> Handle(DeleteWorkoutHistoryCommand request, CancellationToken cancellationToken)
        {
            var workoutHistory = await _workoutHistoryRepository.GetWorkoutHistoryByIdAsync(request.Id);

            if (workoutHistory == null)
                return false;

            await _workoutHistoryRepository.DeleteWorkoutHistoryAsync(workoutHistory);
            return true;
        }
    }
}
