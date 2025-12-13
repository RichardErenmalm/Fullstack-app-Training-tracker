using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
