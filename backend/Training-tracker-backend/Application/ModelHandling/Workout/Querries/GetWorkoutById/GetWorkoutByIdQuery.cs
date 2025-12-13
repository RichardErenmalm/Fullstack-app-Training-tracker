using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Workout.Querries.GetWorkoutById
{
    public class GetWorkoutByIdQuery : IRequest<WorkoutDto>
    {
        public int Id { get; set; }
    }
}
