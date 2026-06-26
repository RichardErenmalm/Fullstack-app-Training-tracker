using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.ModelHandling.WorkoutHistory.Querries.GetWorkoutHistoryById
{
    public class GetWorkoutHistoryByIdQueryHandler
        : IRequestHandler<GetWorkoutHistoryByIdQuery, WorkoutHistoryDto>
    {
        private readonly IWorkoutHistoryRepository _workoutHistoryRepository;
        private readonly IMapper _mapper;

        public GetWorkoutHistoryByIdQueryHandler(IWorkoutHistoryRepository workoutHistoryRepository, IMapper mapper)
        {
            _workoutHistoryRepository = workoutHistoryRepository;
            _mapper = mapper;
        }

        public async Task<WorkoutHistoryDto> Handle(GetWorkoutHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var workoutHistory = await _workoutHistoryRepository.GetWorkoutHistoryByIdAsync(request.Id);

            if (workoutHistory == null)
                return null;

            return _mapper.Map<WorkoutHistoryDto>(workoutHistory);
        }
    }
}
