using Application.Dtos;
using Application.ModelHandling.ExerciseHistories.Commands.CreateExerciseHistory;
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
    public class ExerciseHistoryProfile : Profile
    {

        public ExerciseHistoryProfile()
        {
            CreateMap<ExerciseHistory, ExerciseHistoryDto>();

            CreateMap<CreateExerciseHistoryCommand, ExerciseHistory>();
        }
    }
}
