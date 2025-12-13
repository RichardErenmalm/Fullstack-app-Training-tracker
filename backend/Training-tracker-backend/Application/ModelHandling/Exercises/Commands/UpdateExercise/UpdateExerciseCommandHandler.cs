using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Exercises.Commands.UpdateExercise
{
    public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, ExerciseDto>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;

        public UpdateExerciseCommandHandler(IExerciseRepository exerciseRepository, IMapper mapper)
        {
            _exerciseRepository = exerciseRepository;
            _mapper = mapper;
        }

        public async Task<ExerciseDto> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(request.Id);

            if (exercise == null)
                throw new KeyNotFoundException($"Exercise with Id {request.Id} does not exist");

            exercise.Name = request.Name;

            await _exerciseRepository.UpdateExerciseAsync(exercise);

            return _mapper.Map<ExerciseDto>(exercise);
        }
    }
}
