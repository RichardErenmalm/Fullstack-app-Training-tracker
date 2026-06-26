using Application.Dtos;
using Domain.Common;
using MediatR;

namespace Application.ModelHandling.WorkoutHistory.Querries.GetAllWorkoutHistories
{
    public class GetAllWorkoutHistoriesQuery : IRequest<OperationResult<List<WorkoutHistoryDto>>>;
}
