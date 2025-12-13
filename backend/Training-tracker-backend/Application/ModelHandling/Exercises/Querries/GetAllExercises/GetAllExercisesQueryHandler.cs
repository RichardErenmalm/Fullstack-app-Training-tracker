using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Exercises.Querries.GetAllExercises
{
    public class GetAllExercisesQueryHandler : IRequestHandler<GetAllExercisesQuery, OperationResult<List<ExerciseDto>>>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;

        public GetAllExercisesQueryHandler(IExerciseRepository exerciseRepository, IMapper mapper)
        {
            _exerciseRepository = exerciseRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<ExerciseDto>>> Handle(GetAllExercisesQuery request, CancellationToken cancellationToken)
        {
            var exercises = await _exerciseRepository.GetAllExercisesAsync();
            var exerciseDtos = _mapper.Map<List<ExerciseDto>>(exercises);

            return OperationResult<List<ExerciseDto>>.Success(exerciseDtos, "Fetched exercises successfully");
        }
    }
}
