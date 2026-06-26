using Application.Dtos;
using MediatR;

namespace Application.ModelHandling.WorkoutHistory.Querries.GetWorkoutHistoryById
{
    public class GetWorkoutHistoryByIdQuery : IRequest<WorkoutHistoryDto>
    {
        public int Id { get; set; }
    }
}
