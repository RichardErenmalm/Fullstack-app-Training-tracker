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

namespace Application.ModelHandling.ExerciseHistories.Commands.UpdateExerciseHistory
{
    public class UpdateExerciseHistoryCommandHandler : IRequestHandler<UpdateExerciseHistoryCommand, OperationResult<ExerciseHistoryDto>>
    {
        private readonly IExerciseHistoryRepository _exerciseHistoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;

        public UpdateExerciseHistoryCommandHandler(IExerciseHistoryRepository exerciseHistoryRepository, IUserRepository userRepository,IExerciseRepository exerciseRepository,IMapper mapper)
        {
            _exerciseHistoryRepository = exerciseHistoryRepository;
            _exerciseRepository = exerciseRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //kolla efter userId och ExerciseId
        public async Task<OperationResult<ExerciseHistoryDto>> Handle(UpdateExerciseHistoryCommand request, CancellationToken cancellationToken)
        {
           
            



            var exerciseHistory = await _exerciseHistoryRepository.GetExerciseHistoryByIdAsync(request.Id);

           

            if (exerciseHistory == null)
                throw new KeyNotFoundException($"ExerciseHistory with Id {request.Id} does not exist");


            if (!await _userRepository.ExistsAsync(request.UserId))
            {
                return OperationResult<ExerciseHistoryDto>.Failure(
                 $"User with ID {request.UserId} does not exist.");

            }

            if (!await _exerciseRepository.ExistsAsync(request.ExerciseId))
            {
                return OperationResult<ExerciseHistoryDto>.Failure(
                 $"Exercise with ID {request.ExerciseId} does not exist.");

            }


            exerciseHistory.Id = request.Id;
            exerciseHistory.WeightKg = request.WeightKg;
            exerciseHistory.Reps = request.Reps;
            exerciseHistory.SetNumber = request.SetNumber;
            exerciseHistory.PerformedAt = request.DateTime;
            exerciseHistory.UserId = request.UserId;
            exerciseHistory.ExerciseId = request.ExerciseId;



            await _exerciseHistoryRepository.UpdateExerciseHistoryAsync(exerciseHistory);

            var result = _mapper.Map<ExerciseHistoryDto>(exerciseHistory);

            return OperationResult<ExerciseHistoryDto>.Success(result);


        }
    }
}
