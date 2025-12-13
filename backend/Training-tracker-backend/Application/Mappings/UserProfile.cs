using Application.Dtos;
using Application.ModelHandling.Exercises.Commands.CreateExercise;
using Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ModelHandling.Users.Commands.CreateUser;

namespace Application.Mappings
{
    public class UserProfile : Profile
    {

        public UserProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<CreateUserCommand, User>();
        }
    }
}
