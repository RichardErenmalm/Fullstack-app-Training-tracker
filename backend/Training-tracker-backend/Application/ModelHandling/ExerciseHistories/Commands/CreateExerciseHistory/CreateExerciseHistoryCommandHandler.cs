using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.ExerciseHistories.Commands.CreateExerciseHistory
{
    public class CreateExerciseHistoryCommandHandler
    : IRequestHandler<CreateExerciseHistoryCommand, OperationResult<ExerciseHistoryDto>>
    {
        private readonly IExerciseHistoryRepository _exerciseHistoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;

        public CreateExerciseHistoryCommandHandler(
            IExerciseHistoryRepository exerciseHistoryRepository,
            IUserRepository userRepository,
            IExerciseRepository exerciseRepository,
            IMapper mapper)
        {
            _exerciseHistoryRepository = exerciseHistoryRepository;
            _userRepository = userRepository;
            _exerciseRepository = exerciseRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<ExerciseHistoryDto>> Handle(
            CreateExerciseHistoryCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Kolla om User finns
            var userExists = await _userRepository.ExistsAsync(request.UserId);
            if (!userExists)
                return OperationResult<ExerciseHistoryDto>.Failure(
                    $"User with Id {request.UserId} does not exist");

            // 2. Kolla om Exercise finns
            var exerciseExists = await _exerciseRepository.ExistsAsync(request.ExerciseId);
            if (!exerciseExists)
                return OperationResult<ExerciseHistoryDto>.Failure(
                    $"Exercise with Id {request.ExerciseId} does not exist");

            // 3. Hämta exercisen (för att kopiera namn)
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(request.ExerciseId);

            // 4. Mappa request → model
            var exerciseHistory = _mapper.Map<Domain.Models.ExerciseHistory>(request);

            // sätt namn till exercise.Name
            exerciseHistory.Name = exercise.Name;

            // 5. Spara till DB
            await _exerciseHistoryRepository.AddExerciseHistoryAsync(exerciseHistory);

            // 6. Mappa tillbaka result
            var dto = _mapper.Map<ExerciseHistoryDto>(exerciseHistory);

            return OperationResult<ExerciseHistoryDto>.Success(dto);
        }
    }

}
