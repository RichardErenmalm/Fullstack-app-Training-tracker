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

namespace Application.ModelHandling.WorkoutExercise.Commands.CreateWorkoutExercise
{
    public class CreateWorkoutExerciseCommandHandler
    : IRequestHandler<CreateWorkoutExerciseCommand, OperationResult<WorkoutExerciseDto>>
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
        private readonly IMapper _mapper;

        public CreateWorkoutExerciseCommandHandler(
            IWorkoutExerciseRepository workoutExerciseRepository,
            IMapper mapper)
        {
            _workoutExerciseRepository = workoutExerciseRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<WorkoutExerciseDto>> Handle(
            CreateWorkoutExerciseCommand request,
            CancellationToken cancellationToken)
        {
            var workoutExists = await _workoutExerciseRepository.WorkoutExistsAsync(request.WorkoutId);
            var exerciseExists = await _workoutExerciseRepository.ExerciseExistsAsync(request.ExerciseId);

            if (!workoutExists)
            {
                return OperationResult<WorkoutExerciseDto>.Failure($"Workout with Id {request.WorkoutId} does not exist");
            }
            if (!exerciseExists)
            {
                return OperationResult<WorkoutExerciseDto>.Failure($"Exercise with Id {request.ExerciseId} does not exist");
            }

            var exerciseAlreadyAdded = await _workoutExerciseRepository.ExerciseAlreadyAddedToWorkoutAsync(request.WorkoutId, request.ExerciseId);

            if (exerciseAlreadyAdded)
            {
                return OperationResult<WorkoutExerciseDto>.Failure($"This exercise is already added to this workout");
            }

            var workoutExercise = _mapper.Map<Domain.Models.WorkoutExercise>(request);

            // Spara till DB
            await _workoutExerciseRepository.AddWorkoutExerciseAsync(workoutExercise);

            //workoutExercise.Workout = null;
            //workoutExercise.Exercise = null;

     
            var result = _mapper.Map<WorkoutExerciseDto>(workoutExercise);

            return OperationResult<WorkoutExerciseDto>.Success(result);
        }
    }

}
