using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.ExerciseHistories.Querries.GetExerciseHistoryById
{
    public class GetExerciseHistoryByIdQueryHandler : IRequestHandler<GetExerciseHistoryByIdQuery, ExerciseHistoryDto>
    {
        private readonly IExerciseHistoryRepository _exerciseHistoryRepository;
        private readonly IMapper _mapper;

        public GetExerciseHistoryByIdQueryHandler(IExerciseHistoryRepository exerciseHistoryRepository, IMapper mapper)
        {
            _exerciseHistoryRepository = exerciseHistoryRepository;
            _mapper = mapper;
        }

        public async Task<ExerciseHistoryDto> Handle(GetExerciseHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var exerciseHistory = await _exerciseHistoryRepository.GetExerciseHistoryByIdAsync(request.Id);

            if (exerciseHistory == null)
            {
                return null;
            }

            return _mapper.Map<ExerciseHistoryDto>(exerciseHistory);
        }
    }

}
