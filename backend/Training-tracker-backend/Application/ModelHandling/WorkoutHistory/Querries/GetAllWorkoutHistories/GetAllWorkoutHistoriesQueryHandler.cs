using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using MediatR;

namespace Application.ModelHandling.WorkoutHistory.Querries.GetAllWorkoutHistories
{
    public class GetAllWorkoutHistoriesQueryHandler
        : IRequestHandler<GetAllWorkoutHistoriesQuery, OperationResult<List<WorkoutHistoryDto>>>
    {
        private readonly IWorkoutHistoryRepository _workoutHistoryRepository;
        private readonly IMapper _mapper;

        public GetAllWorkoutHistoriesQueryHandler(IWorkoutHistoryRepository workoutHistoryRepository, IMapper mapper)
        {
            _workoutHistoryRepository = workoutHistoryRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<WorkoutHistoryDto>>> Handle(
            GetAllWorkoutHistoriesQuery request,
            CancellationToken cancellationToken)
        {
            var histories = await _workoutHistoryRepository.GetAllWorkoutHistoriesAsync();
            var dtos = _mapper.Map<List<WorkoutHistoryDto>>(histories);
            return OperationResult<List<WorkoutHistoryDto>>.Success(dtos, "Fetched workout histories successfully");
        }
    }
}
