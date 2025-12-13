using Application.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.ExerciseHistories.Commands.CreateExerciseHistory
{
    public class CreateExerciseHistoryCommand : IRequest<OperationResult<ExerciseHistoryDto>>
    {

        public int WeightKg { get; set; }

        public int Reps { get; set; }

        public int SetNumber { get; set; }

        public int ExerciseId { get; set; }

        public int UserId { get; set; }

        public DateTime DatTime = DateTime.Now;
    }
}
