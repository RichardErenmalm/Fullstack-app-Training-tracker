using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Workout.Commands.UpdateWorkout
{
    public class UpdateWorkoutCommandHandler
     : IRequestHandler<UpdateWorkoutCommand, OperationResult<WorkoutDto>>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateWorkoutCommandHandler(
            IWorkoutRepository workoutRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _workoutRepository = workoutRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<WorkoutDto>> Handle(UpdateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = await _workoutRepository.GetWorkoutByIdAsync(request.Id);

            if (workout == null)
                throw new KeyNotFoundException($"Workout with Id {request.Id} does not exist");

            // Validate UserId
            if (!await _userRepository.ExistsAsync(request.UserId))
            {
                return OperationResult<WorkoutDto>.Failure(
                    $"User with ID {request.UserId} does not exist.");
            }

            // Update values
            workout.Id = request.Id;
            workout.Name = request.Name; 
            workout.UserId = request.UserId;

            await _workoutRepository.UpdateWorkoutAsync(workout);

            var result = _mapper.Map<WorkoutDto>(workout);

            return OperationResult<WorkoutDto>.Success(result);
        }
    }

}
