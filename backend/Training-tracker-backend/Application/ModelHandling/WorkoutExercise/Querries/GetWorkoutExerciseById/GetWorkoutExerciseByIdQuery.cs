using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.WorkoutExercise.Querries.GetWorkoutExerciseById
{

    public class GetWorkoutExerciseByIdQuery : IRequest<WorkoutExerciseDto>
    {
        public int Id { get; set; }
    }

}
