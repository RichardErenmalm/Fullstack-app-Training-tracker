using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.WorkoutExercise.Querries.GetWorkoutExerciseById
{
    public class GetWorkoutExerciseByIdQueryHandler
     : IRequestHandler<GetWorkoutExerciseByIdQuery, WorkoutExerciseDto>
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
        private readonly IMapper _mapper;

        public GetWorkoutExerciseByIdQueryHandler(
            IWorkoutExerciseRepository workoutExerciseRepository,
            IMapper mapper)
        {
            _workoutExerciseRepository = workoutExerciseRepository;
            _mapper = mapper;
        }

        public async Task<WorkoutExerciseDto> Handle(
            GetWorkoutExerciseByIdQuery request,
            CancellationToken cancellationToken)
        {
            var workoutExercise = await _workoutExerciseRepository
                .GetWorkoutExerciseByIdAsync(request.Id);

            if (workoutExercise == null)
            {
                return null;
            }

            return _mapper.Map<WorkoutExerciseDto>(workoutExercise);
        }
    }

}
