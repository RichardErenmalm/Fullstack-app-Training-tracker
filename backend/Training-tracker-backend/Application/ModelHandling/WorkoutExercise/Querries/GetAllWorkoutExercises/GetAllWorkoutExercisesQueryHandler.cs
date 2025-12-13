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

namespace Application.ModelHandling.WorkoutExercise.Querries.GetAllWorkoutExercises
{
    public class GetAllWorkoutExercisesQueryHandler
     : IRequestHandler<GetAllWorkoutExercisesQuery, OperationResult<List<WorkoutExerciseDto>>>
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
        private readonly IMapper _mapper;

        public GetAllWorkoutExercisesQueryHandler(
            IWorkoutExerciseRepository workoutExerciseRepository,
            IMapper mapper)
        {
            _workoutExerciseRepository = workoutExerciseRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<WorkoutExerciseDto>>> Handle(
            GetAllWorkoutExercisesQuery request,
            CancellationToken cancellationToken)
        {
            var workoutExercises = await _workoutExerciseRepository.GetAllWorkoutExercisesAsync();
            var workoutExerciseDtos = _mapper.Map<List<WorkoutExerciseDto>>(workoutExercises);

            return OperationResult<List<WorkoutExerciseDto>>
                .Success(workoutExerciseDtos, "Fetched workout exercises successfully");
        }
    }

}
