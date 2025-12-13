using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.ExerciseHistories.Commands.DeleteExerciseHistory
{
    public class DeleteExerciseHistoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
