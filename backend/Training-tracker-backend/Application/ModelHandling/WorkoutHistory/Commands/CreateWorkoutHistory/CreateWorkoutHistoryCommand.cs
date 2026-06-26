using Application.Dtos;
using Domain.Common;
using MediatR;

namespace Application.ModelHandling.WorkoutHistory.Commands.CreateWorkoutHistory
{
    public class CreateWorkoutHistoryCommand : IRequest<OperationResult<WorkoutHistoryDto>>
    {
        public int WorkoutId { get; set; }
        public int UserId { get; set; }
    }
}
