using Application.ModelHandling.WorkoutHistory.Commands.CreateWorkoutHistory;
using Application.ModelHandling.WorkoutHistory.Commands.DeleteWorkoutHistory;
using Application.ModelHandling.WorkoutHistory.Querries.GetAllWorkoutHistories;
using Application.ModelHandling.WorkoutHistory.Querries.GetWorkoutHistoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutHistoryController : ControllerBase
    {
        private readonly ILogger<WorkoutHistoryController> _logger;
        private readonly IMediator _mediator;

        public WorkoutHistoryController(ILogger<WorkoutHistoryController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutHistories()
        {
            _logger.LogInformation("Fetching all workout histories");
            var result = await _mediator.Send(new GetAllWorkoutHistoriesQuery());

            if (result == null || !result.IsSuccess)
            {
                _logger.LogWarning("No workout histories found or an error occurred");
                return NotFound(result);
            }

            _logger.LogInformation("{Count} workout histories retrieved", result.Data?.Count ?? 0);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutHistoryById(int id)
        {
            _logger.LogInformation("Fetching workout history with id {Id}", id);
            var result = await _mediator.Send(new GetWorkoutHistoryByIdQuery { Id = id });

            if (result == null)
            {
                _logger.LogWarning("Workout history with id {Id} not found", id);
                return NotFound(new { message = $"WorkoutHistory with Id {id} does not exist" });
            }

            _logger.LogInformation("Workout history with id {Id} retrieved", id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkoutHistory(CreateWorkoutHistoryCommand command)
        {
            _logger.LogInformation("Creating workout history for workout id {WorkoutId}", command.WorkoutId);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to create workout history: {Error}", result.ErrorMessage);
                return BadRequest(result);
            }

            _logger.LogInformation("Workout history created with id {Id}", result.Data.Id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutHistory(int id, [FromBody] DeleteWorkoutHistoryCommand command)
        {
            _logger.LogInformation("Deleting workout history with id {Id}", id);

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
                    _logger.LogWarning("Workout history with id {Id} not found", id);
                    return NotFound(new { success = false, message = $"WorkoutHistory with Id {id} not found." });
                }

                _logger.LogInformation("Workout history with id {Id} deleted successfully", id);
                return Ok(new { success = true, message = $"WorkoutHistory with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting workout history with id {Id}", id);
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
}
