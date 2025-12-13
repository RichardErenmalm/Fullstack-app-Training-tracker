using Application.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.ExerciseHistories.Commands.UpdateExerciseHistory
{
    public class UpdateExerciseHistoryCommand : IRequest<OperationResult<ExerciseHistoryDto>>
    {
        public int Id { get; set; }

        public int WeightKg { get; set; }

        public int Reps { get; set; }

        public int SetNumber { get; set; }

        public DateTime DateTime { get; set; }

        public int UserId { get; set; }
        public int ExerciseId { get; set; }
    }
}
