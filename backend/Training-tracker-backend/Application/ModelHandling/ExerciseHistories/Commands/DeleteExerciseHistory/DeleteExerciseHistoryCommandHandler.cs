using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelHandling.ExerciseHistories.Commands.DeleteExerciseHistory
{
    public class DeleteExerciseHistoryCommandHandler : IRequestHandler<DeleteExerciseHistoryCommand, bool>
    {
        private readonly IExerciseHistoryRepository _exerciseHistoryRepository;

        public DeleteExerciseHistoryCommandHandler(IExerciseHistoryRepository exerciseHistoryRepository)
        {
            _exerciseHistoryRepository = exerciseHistoryRepository;
        }

        public async Task<bool> Handle(DeleteExerciseHistoryCommand request, CancellationToken cancellationToken)
        {
            var exerciseHistory = await _exerciseHistoryRepository.GetExerciseHistoryByIdAsync(request.Id);
            if (exerciseHistory == null)
            {
                return false;
            }

            await _exerciseHistoryRepository.DeleteExerciseHistoryAsync(exerciseHistory);
            return true;
        }
    }

}
