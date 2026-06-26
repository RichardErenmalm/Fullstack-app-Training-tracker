using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using MediatR;

namespace Application.ModelHandling.WorkoutHistory.Commands.CreateWorkoutHistory
{
    public class CreateWorkoutHistoryCommandHandler
        : IRequestHandler<CreateWorkoutHistoryCommand, OperationResult<WorkoutHistoryDto>>
    {
        private readonly IWorkoutHistoryRepository _workoutHistoryRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateWorkoutHistoryCommandHandler(
            IWorkoutHistoryRepository workoutHistoryRepository,
            IWorkoutRepository workoutRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _workoutHistoryRepository = workoutHistoryRepository;
            _workoutRepository = workoutRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<WorkoutHistoryDto>> Handle(
            CreateWorkoutHistoryCommand request,
            CancellationToken cancellationToken)
        {
            if (!await _userRepository.ExistsAsync(request.UserId))
                return OperationResult<WorkoutHistoryDto>.Failure(
                    $"User with Id {request.UserId} does not exist");

            if (!await _workoutRepository.ExistsAsync(request.WorkoutId))
                return OperationResult<WorkoutHistoryDto>.Failure(
                    $"Workout with Id {request.WorkoutId} does not exist");

            var workoutHistory = _mapper.Map<Domain.Models.WorkoutHistory>(request);

            await _workoutHistoryRepository.AddWorkoutHistoryAsync(workoutHistory);

            var dto = _mapper.Map<WorkoutHistoryDto>(workoutHistory);

            return OperationResult<WorkoutHistoryDto>.Success(dto);
        }
    }
}
