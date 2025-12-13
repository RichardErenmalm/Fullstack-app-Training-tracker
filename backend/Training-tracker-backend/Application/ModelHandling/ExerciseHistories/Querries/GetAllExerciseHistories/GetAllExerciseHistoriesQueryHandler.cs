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

namespace Application.ModelHandling.ExerciseHistories.Querries.GetAllExerciseHistories
{
    public class GetAllExerciseHistoriesQueryHandler : IRequestHandler<GetAllExerciseHistoriesQuery, OperationResult<List<ExerciseHistoryDto>>>
    {
        private readonly IExerciseHistoryRepository _exerciseHistoryRepository;
        private readonly IMapper _mapper;

        public GetAllExerciseHistoriesQueryHandler(IExerciseHistoryRepository exerciseHistoryRepository, IMapper mapper)
        {
            _exerciseHistoryRepository = exerciseHistoryRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<ExerciseHistoryDto>>> Handle(GetAllExerciseHistoriesQuery request, CancellationToken cancellationToken)
        {
            var exerciseHistories = await _exerciseHistoryRepository.GetAllExerciseHistoriesAsync();
            var exerciseHistoryDtos = _mapper.Map<List<ExerciseHistoryDto>>(exerciseHistories);

            return OperationResult<List<ExerciseHistoryDto>>.Success(exerciseHistoryDtos, "Fetched exercise histories successfully");
        }
    }

}
