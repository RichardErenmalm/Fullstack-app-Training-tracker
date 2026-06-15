using Application.Interfaces;
using MediatR;

namespace Application.ModelHandling.Workout.Commands.DeleteWorkout
{
    public class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand, bool>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
        private readonly IWorkoutHistoryRepository _workoutHistoryRepository;
        private readonly IExerciseHistoryRepository _exerciseHistoryRepository;

        public DeleteWorkoutCommandHandler(
            IWorkoutRepository workoutRepository,
            IWorkoutExerciseRepository workoutExerciseRepository,
            IWorkoutHistoryRepository workoutHistoryRepository,
            IExerciseHistoryRepository exerciseHistoryRepository)
        {
            _workoutRepository = workoutRepository;
            _workoutExerciseRepository = workoutExerciseRepository;
            _workoutHistoryRepository = workoutHistoryRepository;
            _exerciseHistoryRepository = exerciseHistoryRepository;
        }

        public async Task<bool> Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = await _workoutRepository.GetWorkoutByIdAsync(request.Id);

            if (workout == null)
                return false;

            // 1. Get all WorkoutExercise ids for this workout
            var workoutExercises = await _workoutExerciseRepository.GetWorkoutExercisesByWorkoutIdAsync(request.Id);
            var weIds = workoutExercises.Select(we => we.Id).ToList();

            // 2. Detach ExerciseHistories from WorkoutExercises (set FK to null)
            if (weIds.Any())
                await _exerciseHistoryRepository.DetachFromWorkoutExercisesAsync(weIds);

            // 3. Detach WorkoutHistories from Workout (set WorkoutId to null)
            await _workoutHistoryRepository.DetachFromWorkoutAsync(request.Id);

            // 4. Delete WorkoutExercises manually (no cascade anymore)
            foreach (var we in workoutExercises)
                await _workoutExerciseRepository.DeleteWorkoutExerciseAsync(we);

            // 5. Delete the Workout
            await _workoutRepository.DeleteWorkoutAsync(workout);

            return true;
        }
    }
}
