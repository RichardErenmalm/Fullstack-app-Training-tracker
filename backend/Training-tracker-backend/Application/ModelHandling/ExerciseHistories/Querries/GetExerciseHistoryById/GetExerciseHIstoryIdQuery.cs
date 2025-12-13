using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.ExerciseHistories.Querries.GetExerciseHistoryById
{
    public class GetExerciseHistoryByIdQuery : IRequest<ExerciseHistoryDto>
    {
        public int Id { get; set; }
    }
}
