using Application.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.Exercises.Querries.GetAllExercises
{
    public class GetAllExercisesQuery : IRequest<OperationResult<List<ExerciseDto>>>;
   
}
