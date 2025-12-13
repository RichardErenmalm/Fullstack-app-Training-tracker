using Application.ModelHandling.ExerciseHistories.Commands.CreateExerciseHistory;
using Application.ModelHandling.ExerciseHistories.Commands.DeleteExerciseHistory;
using Application.ModelHandling.ExerciseHistories.Commands.UpdateExerciseHistory;
using Application.ModelHandling.ExerciseHistories.Querries.GetAllExerciseHistories;
using Application.ModelHandling.ExerciseHistories.Querries.GetExerciseHistoryById;
using Application.ModelHandling.Exercises.Commands.CreateExercise;
using Application.ModelHandling.Exercises.Commands.DeleteExercise;
using Application.ModelHandling.Exercises.Commands.UpdateExercise;
using Application.ModelHandling.Exercises.Querries.GetAllExercises;
using Application.ModelHandling.Exercises.Querries.GetExerciseById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ExerciseHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExerciseHistories() =>
            Ok(await _mediator.Send(new GetAllExerciseHistoriesQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExerciseHistoryById(int id)
        {
            var result = await _mediator.Send(new GetExerciseHistoryByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { message = $"Exercise history with Id {id} does not exist" });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExerciseHistory(CreateExerciseHistoryCommand command) =>
          Ok(await _mediator.Send(command));


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExerciseHistory(int id, [FromBody] UpdateExerciseHistoryCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { message = "Route id and body id must match" });

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExerciseHistory(int id, [FromBody] DeleteExerciseHistoryCommand command)
        {
            if (command.Id != id)
            {
                return BadRequest(new { success = false, message = $"Id mismatch: {id} != {command.Id}" });
            }

            try
            {
                var result = await _mediator.Send(command);

                if (!result)
                {
                    return NotFound(new { success = false, message = $"ExerciseHistory with Id {id} not found or could not be deleted." });
                }

                return Ok(new { success = true, message = $"ExerciseHistory with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error deleting ExerciseHistory: {ex.Message}" });
            }
        }



    }
}
