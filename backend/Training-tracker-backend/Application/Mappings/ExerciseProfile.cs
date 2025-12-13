using Application.Dtos;
using Application.ModelHandling.Exercises.Commands.CreateExercise;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class ExerciseProfile : Profile
    {
        public ExerciseProfile()
        {
            CreateMap<Exercise, ExerciseDto>();

            CreateMap<CreateExerciseCommand, Exercise>();
        }
    }
}
