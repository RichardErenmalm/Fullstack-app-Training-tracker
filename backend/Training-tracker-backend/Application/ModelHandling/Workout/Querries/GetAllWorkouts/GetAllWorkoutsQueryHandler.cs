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

namespace Application.ModelHandling.Workout.Querries.GetAllWorkouts
{
    public class GetAllWorkoutsQueryHandler : IRequestHandler<GetAllWorkoutsQuery, OperationResult<List<WorkoutDto>>>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IMapper _mapper;

        public GetAllWorkoutsQueryHandler(IWorkoutRepository workoutRepository, IMapper mapper)
        {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<WorkoutDto>>> Handle(GetAllWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var workouts = await _workoutRepository.GetAllWorkoutsAsync();
            var workoutDtos = _mapper.Map<List<WorkoutDto>>(workouts);

            return OperationResult<List<WorkoutDto>>.Success(workoutDtos, "Fetched workouts successfully");
        }
    }

}
