using Application.Dtos;
using Application.ModelHandling.Workout.Commands.CreateWorkout;
using Application.ModelHandling.WorkoutExercise.Commands.CreateWorkoutExercise;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class WorkoutExerciseProfile : Profile
    {

        public WorkoutExerciseProfile()
        {
            CreateMap<WorkoutExercise, WorkoutExerciseDto>();

            CreateMap<CreateWorkoutExerciseCommand, WorkoutExercise>()
                .ForMember(dest => dest.Workout, opt => opt.Ignore())
                .ForMember(dest => dest.Exercise, opt => opt.Ignore());
        }
    }
}
