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

namespace Application.ModelHandling.Workout.Commands.CreateWorkout
{
    public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, OperationResult<WorkoutDto>>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateWorkoutCommandHandler(
            IWorkoutRepository workoutRepository,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        // Kontrollera att user och workout finns. 
        // Gör så att workoutHistory.Name = workout.Name.
        public async Task<OperationResult<WorkoutDto>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            // Kontrollera att användaren finns
            if (!await _userRepository.ExistsAsync(request.UserId))
            {
                return OperationResult<WorkoutDto>.Failure(
                    $"User with ID {request.UserId} does not exist.");
            }

            // Mappa kommandot till domänmodell
            var workout = _mapper.Map<Domain.Models.Workout>(request);

            workout.User = null;

            // Spara i databasen
            await _workoutRepository.AddWorkoutAsync(workout);

            // Mappa tillbaka till DTO (Observera att AutoMapper måste inkludera Id)
            var result = _mapper.Map<WorkoutDto>(workout);

            // Returnera som Success, med ID
            return OperationResult<WorkoutDto>.Success(result);
        }

    }



}
