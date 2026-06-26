using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using MediatR;

namespace Application.ModelHandling.WorkoutExercise.Commands.UpdateWorkoutExercise
{
    public class UpdateWorkoutExerciseCommandHandler
    : IRequestHandler<UpdateWorkoutExerciseCommand, OperationResult<WorkoutExerciseDto>>
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
        private readonly IMapper _mapper;

        public UpdateWorkoutExerciseCommandHandler(
            IWorkoutExerciseRepository workoutExerciseRepository,
            IMapper mapper)
        {
            _workoutExerciseRepository = workoutExerciseRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<WorkoutExerciseDto>> Handle(
            UpdateWorkoutExerciseCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Sets <= 0)
                return OperationResult<WorkoutExerciseDto>.Failure("Sets must be greater than 0.");

            var workoutExercise = await _workoutExerciseRepository.GetWorkoutExerciseByIdAsync(request.Id);

            if (workoutExercise == null)
                return OperationResult<WorkoutExerciseDto>.Failure(
                    $"WorkoutExercise with Id {request.Id} does not exist");

            if (!await _workoutExerciseRepository.WorkoutExistsAsync(request.WorkoutId))
                return OperationResult<WorkoutExerciseDto>.Failure(
                    $"Workout with ID {request.WorkoutId} does not exist.");

            if (!await _workoutExerciseRepository.ExerciseExistsAsync(request.ExerciseId))
                return OperationResult<WorkoutExerciseDto>.Failure(
                    $"Exercise with ID {request.ExerciseId} does not exist.");

            var duplicateExists = await _workoutExerciseRepository
                .ExerciseAlreadyAddedToWorkoutUpdateAsync(request.WorkoutId, request.ExerciseId, request.Id);

            if (duplicateExists)
                return OperationResult<WorkoutExerciseDto>.Failure(
                    "This exercise is already added to this workout.");

            workoutExercise.WorkoutId = request.WorkoutId;
            workoutExercise.ExerciseId = request.ExerciseId;
            workoutExercise.Sets = request.Sets;

            await _workoutExerciseRepository.UpdateWorkoutExerciseAsync(workoutExercise);

            var result = _mapper.Map<WorkoutExerciseDto>(workoutExercise);
            return OperationResult<WorkoutExerciseDto>.Success(result);
        }
    }
}
