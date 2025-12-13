using Application.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Users.Querries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<OperationResult<List<UserDto>>>;
}
