using Application.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.WorkoutExercise.Querries.GetWorkoutExercisesByWorkoutId
{
    public class GetWorkoutExercisesByWorkoutIdQuery
     : IRequest<OperationResult<List<WorkoutExerciseDto>>>
    {
        public int WorkoutId { get; }

        public GetWorkoutExercisesByWorkoutIdQuery(int workoutId)
        {
            WorkoutId = workoutId;
        }
    }

}
