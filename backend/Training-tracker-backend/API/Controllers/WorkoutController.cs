using Application.ModelHandling.Workout.Commands.CreateWorkout;
using Application.ModelHandling.Workout.Commands.DeleteWorkout;
using Application.ModelHandling.Workout.Commands.UpdateWorkout;
using Application.ModelHandling.Workout.Querries.GetAllWorkouts;
using Application.ModelHandling.Workout.Querries.GetWorkoutById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/Workouts")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly ILogger<WorkoutController> _logger;
        private readonly IMediator _mediator;

        public WorkoutController(ILogger<WorkoutController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkouts()
        {
            _logger.LogInformation("Fetching all workouts");

            var result = await _mediator.Send(new GetAllWorkoutsQuery());

            if (result == null || !result.IsSuccess)
            {
                _logger.LogWarning("No workouts found or an error occurred");
                return NotFound(result);
            }

            _logger.LogInformation("{Count} workouts retrieved", result.Data?.Count ?? 0);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutById(int id)
        {
            _logger.LogInformation("Fetching workout with id {WorkoutId}", id);

            var result = await _mediator.Send(new GetWorkoutByIdQuery { Id = id });

            if (result == null)
            {
                _logger.LogWarning("Workout with id {WorkoutId} not found", id);
                return NotFound(new { message = $"Workout with Id {id} does not exist" });
            }

            _logger.LogInformation("Workout with id {WorkoutId} retrieved", id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkout(CreateWorkoutCommand command)
        {
            _logger.LogInformation("Creating new workout for user id {UserId}", command.UserId);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to create workout for user id {UserId}: {ErrorMessage}", command.UserId, result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }

            _logger.LogInformation("Workout created successfully with id {WorkoutId}", result.Data.Id);
            return CreatedAtAction(
                nameof(GetWorkoutById),
                new { id = result.Data.Id },
                result.Data
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkout(int id, [FromBody] UpdateWorkoutCommand command)
        {
            _logger.LogInformation("Updating workout with id {WorkoutId}", id);

            if (id != command.Id)
            {
                _logger.LogWarning("Route id {RouteId} does not match body id {BodyId}", id, command.Id);
                return BadRequest(new { message = "Route id and body id must match" });
            }

            var result = await _mediator.Send(command);
            _logger.LogInformation("Workout with id {WorkoutId} updated successfully", id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id, [FromBody] DeleteWorkoutCommand command)
        {
            _logger.LogInformation("Deleting workout with id {WorkoutId}", id);

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
                    _logger.LogWarning("Workout with id {WorkoutId} not found or could not be deleted", id);
                    return NotFound(new { success = false, message = $"Workout with Id {id} not found or could not be deleted." });
                }

                _logger.LogInformation("Workout with id {WorkoutId} deleted successfully", id);
                return Ok(new { success = true, message = $"Workout with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting workout with id {WorkoutId}", id);
                return StatusCode(500, new { success = false, message = $"Error deleting workout: {ex.Message}" });
            }
        }
    }
}
