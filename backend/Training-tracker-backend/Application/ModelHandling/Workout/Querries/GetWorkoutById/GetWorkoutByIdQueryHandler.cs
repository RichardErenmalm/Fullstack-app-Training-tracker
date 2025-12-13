using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Workout.Querries.GetWorkoutById
{
    public class GetWorkoutByIdQueryHandler : IRequestHandler<GetWorkoutByIdQuery, WorkoutDto>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IMapper _mapper;

        public GetWorkoutByIdQueryHandler(IWorkoutRepository workoutRepository, IMapper mapper)
        {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
        }

        public async Task<WorkoutDto> Handle(GetWorkoutByIdQuery request, CancellationToken cancellationToken)
        {
            var workout = await _workoutRepository.GetWorkoutByIdAsync(request.Id);

            if (workout == null)
            {
                return null;
            }

            return _mapper.Map<WorkoutDto>(workout);
        }
    }

}
