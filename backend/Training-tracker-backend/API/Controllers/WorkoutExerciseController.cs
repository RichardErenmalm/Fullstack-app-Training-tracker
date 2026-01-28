using Application.ModelHandling.WorkoutExercise.Commands.CreateWorkoutExercise;
using Application.ModelHandling.WorkoutExercise.Commands.DeleteWorkoutExercise;
using Application.ModelHandling.WorkoutExercise.Commands.UpdateWorkoutExercise;
using Application.ModelHandling.WorkoutExercise.Querries.GetAllWorkoutExercises;
using Application.ModelHandling.WorkoutExercise.Querries.GetWorkoutExerciseById;
using Application.ModelHandling.WorkoutExercise.Querries.GetWorkoutExercisesByWorkoutId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/WorkoutExercises")]
    [ApiController]
    public class WorkoutExerciseController : ControllerBase
    {
        private readonly ILogger<WorkoutExerciseController> _logger;
        private readonly IMediator _mediator;

        public WorkoutExerciseController(ILogger<WorkoutExerciseController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutExercises()
        {
            _logger.LogInformation("Fetching all workout exercises");

            var result = await _mediator.Send(new GetAllWorkoutExercisesQuery());

            if (result == null || !result.IsSuccess)
            {
                _logger.LogWarning("No workout exercises found or an error occurred");
                return NotFound(result);
            }

            _logger.LogInformation("{Count} workout exercises retrieved", result.Data?.Count ?? 0);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutExerciseById(int id)
        {
            _logger.LogInformation("Fetching workout exercise with id {WorkoutExerciseId}", id);

            var result = await _mediator.Send(new GetWorkoutExerciseByIdQuery { Id = id });

            if (result == null)
            {
                _logger.LogWarning("WorkoutExercise with id {WorkoutExerciseId} not found", id);
                return NotFound(new { message = $"WorkoutExercise with Id {id} does not exist" });
            }

            _logger.LogInformation("WorkoutExercise with id {WorkoutExerciseId} retrieved", id);
            return Ok(result);
        }

        [HttpGet("workout/{workoutId}")]
        public async Task<IActionResult> GetWorkoutExercisesByWorkoutId(int workoutId)
        {
            _logger.LogInformation("Fetching workout exercises for workout id {WorkoutId}", workoutId);

            var result = await _mediator.Send(new GetWorkoutExercisesByWorkoutIdQuery(workoutId));

            if (result == null || !result.IsSuccess)
            {
                _logger.LogWarning("No workout exercises found for workout id {WorkoutId}", workoutId);
                return NotFound(result);
            }

            _logger.LogInformation("{Count} workout exercises retrieved for workout id {WorkoutId}", result.Data?.Count ?? 0, workoutId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkoutExercise(CreateWorkoutExerciseCommand command)
        {
            _logger.LogInformation("Creating new workout exercise for workout id {WorkoutId} and exercise id {ExerciseId}", command.WorkoutId, command.ExerciseId);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to create workout exercise for workout id {WorkoutId} and exercise id {ExerciseId}: {ErrorMessage}", command.WorkoutId, command.ExerciseId, result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }

            _logger.LogInformation("Workout exercise created successfully with id {WorkoutExerciseId}", result.Data.Id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkoutExercise(int id, [FromBody] UpdateWorkoutExerciseCommand command)
        {
            _logger.LogInformation("Updating workout exercise with id {WorkoutExerciseId}", id);

            if (id != command.Id)
            {
                _logger.LogWarning("Route id {RouteId} does not match body id {BodyId}", id, command.Id);
                return BadRequest(new { message = "Route id and body id must match" });
            }

            var result = await _mediator.Send(command);
            _logger.LogInformation("WorkoutExercise with id {WorkoutExerciseId} updated successfully", id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutExercise(int id, [FromBody] DeleteWorkoutExerciseCommand command)
        {
            _logger.LogInformation("Deleting workout exercise with id {WorkoutExerciseId}", id);

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
                    _logger.LogWarning("WorkoutExercise with id {WorkoutExerciseId} not found or could not be deleted", id);
                    return NotFound(new { success = false, message = $"WorkoutExercise with Id {id} not found or could not be deleted." });
                }

                _logger.LogInformation("WorkoutExercise with id {WorkoutExerciseId} deleted successfully", id);
                return Ok(new { success = true, message = $"WorkoutExercise with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting WorkoutExercise with id {WorkoutExerciseId}", id);
                return StatusCode(500, new { success = false, message = $"Error deleting WorkoutExercise: {ex.Message}" });
            }
        }
    }
}
