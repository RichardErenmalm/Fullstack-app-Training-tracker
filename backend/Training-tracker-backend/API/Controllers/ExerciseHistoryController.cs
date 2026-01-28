using Application.ModelHandling.ExerciseHistories.Commands.CreateExerciseHistory;
using Application.ModelHandling.ExerciseHistories.Commands.DeleteExerciseHistory;
using Application.ModelHandling.ExerciseHistories.Commands.UpdateExerciseHistory;
using Application.ModelHandling.ExerciseHistories.Querries.GetAllExerciseHistories;
using Application.ModelHandling.ExerciseHistories.Querries.GetExerciseHistoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseHistoryController : ControllerBase
    {
        private readonly ILogger<ExerciseHistoryController> _logger;
        private readonly IMediator _mediator;

        public ExerciseHistoryController(ILogger<ExerciseHistoryController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExerciseHistories()
        {
            _logger.LogInformation("Fetching all exercise histories");

            var result = await _mediator.Send(new GetAllExerciseHistoriesQuery());

            if (result == null || !result.IsSuccess)
            {
                _logger.LogWarning("No exercise histories found or an error occurred");
                return NotFound(result);
            }

            _logger.LogInformation("{Count} exercise histories retrieved", result.Data?.Count ?? 0);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExerciseHistoryById(int id)
        {
            _logger.LogInformation("Fetching exercise history with id {ExerciseHistoryId}", id);

            var result = await _mediator.Send(new GetExerciseHistoryByIdQuery { Id = id });

            if (result == null)
            {
                _logger.LogWarning("Exercise history with id {ExerciseHistoryId} not found", id);
                return NotFound(new { message = $"Exercise history with Id {id} does not exist" });
            }

            _logger.LogInformation("Exercise history with id {ExerciseHistoryId} retrieved", id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExerciseHistory(CreateExerciseHistoryCommand command)
        {
            _logger.LogInformation("Creating new exercise history for exercise id {ExerciseId}", command.ExerciseId);
            var result = await _mediator.Send(command);
            _logger.LogInformation("Exercise history for exercise id {ExerciseId} created successfully", command.ExerciseId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExerciseHistory(int id, [FromBody] UpdateExerciseHistoryCommand command)
        {
            _logger.LogInformation("Updating exercise history with id {ExerciseHistoryId}", id);

            if (id != command.Id)
            {
                _logger.LogWarning("Route id {RouteId} does not match body id {BodyId}", id, command.Id);
                return BadRequest(new { message = "Route id and body id must match" });
            }

            var result = await _mediator.Send(command);
            _logger.LogInformation("Exercise history with id {ExerciseHistoryId} updated successfully", id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExerciseHistory(int id, [FromBody] DeleteExerciseHistoryCommand command)
        {
            _logger.LogInformation("Deleting exercise history with id {ExerciseHistoryId}", id);

            if (command.Id != id)
            {
                _logger.LogWarning("Route id {RouteId} does not match body id {BodyId}", id, command.Id);
                return BadRequest(new { success = false, message = $"Id mismatch: {id} != {command.Id}" });
            }

            try
            {
                var result = await _mediator.Send(command);

                if (!result)
                {
                    _logger.LogWarning("Exercise history with id {ExerciseHistoryId} not found or could not be deleted", id);
                    return NotFound(new { success = false, message = $"ExerciseHistory with Id {id} not found or could not be deleted." });
                }

                _logger.LogInformation("Exercise history with id {ExerciseHistoryId} deleted successfully", id);
                return Ok(new { success = true, message = $"ExerciseHistory with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exercise history with id {ExerciseHistoryId}", id);
                return StatusCode(500, new { success = false, message = $"Error deleting ExerciseHistory: {ex.Message}" });
            }
        }
    }
}
