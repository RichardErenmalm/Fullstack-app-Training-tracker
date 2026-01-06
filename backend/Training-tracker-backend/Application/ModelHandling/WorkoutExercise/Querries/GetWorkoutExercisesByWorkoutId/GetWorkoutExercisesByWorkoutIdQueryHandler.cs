using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using MediatR;

namespace Application.ModelHandling.WorkoutExercise.Querries.GetWorkoutExercisesByWorkoutId
{
    public class GetWorkoutExercisesByWorkoutIdQueryHandler
        : IRequestHandler<
            GetWorkoutExercisesByWorkoutIdQuery,
            OperationResult<List<WorkoutExerciseDto>>>
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
        private readonly IMapper _mapper;

        public GetWorkoutExercisesByWorkoutIdQueryHandler(
            IWorkoutExerciseRepository workoutExerciseRepository,
            IMapper mapper)
        {
            _workoutExerciseRepository = workoutExerciseRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<WorkoutExerciseDto>>> Handle(
            GetWorkoutExercisesByWorkoutIdQuery request,
            CancellationToken cancellationToken)
        {
            var workoutExercises =
                await _workoutExerciseRepository
                    .GetWorkoutExercisesByWorkoutIdAsync(request.WorkoutId);

            var workoutExerciseDtos =
                _mapper.Map<List<WorkoutExerciseDto>>(workoutExercises);

            return OperationResult<List<WorkoutExerciseDto>>
                .Success(workoutExerciseDtos,
                    "Fetched workout exercises for workout successfully");
        }
    }
}
