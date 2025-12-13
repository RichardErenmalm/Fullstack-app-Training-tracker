using Application.Dtos;
using Application.ModelHandling.Workout.Commands.CreateWorkout;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class WorkoutProfile : Profile
    {

        public WorkoutProfile() 
        {
            CreateMap<Workout, WorkoutDto>();

            CreateMap<CreateWorkoutCommand, Workout>();
        }
    }
}
