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

namespace Application.ModelHandling.Exercises.Commands.CreateExercise
{
    public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, ExerciseDto>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;

        public CreateExerciseCommandHandler(IExerciseRepository exerciseRepository, IMapper mapper)
        {
            _exerciseRepository = exerciseRepository;
            _mapper = mapper;
        }

        public async Task<ExerciseDto> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            var exercise = _mapper.Map<Exercise>(request);

            // Spara till DB
            await _exerciseRepository.AddExerciseAsync(exercise);

            // Mappa tillbaka → DTO
            var result = _mapper.Map<ExerciseDto>(exercise);

            return result;
        }
    }
}
