using MediatR;

namespace Application.ModelHandling.WorkoutHistory.Commands.DeleteWorkoutHistory
{
    public class DeleteWorkoutHistoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
