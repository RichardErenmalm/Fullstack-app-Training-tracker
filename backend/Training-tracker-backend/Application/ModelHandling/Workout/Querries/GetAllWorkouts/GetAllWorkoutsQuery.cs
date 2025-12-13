using Application.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Workout.Querries.GetAllWorkouts
{
    public class GetAllWorkoutsQuery : IRequest<OperationResult<List<WorkoutDto>>>;
}
