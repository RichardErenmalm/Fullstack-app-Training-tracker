using Application.ModelHandling.Exercises.Commands.CreateExercise;
using Application.ModelHandling.Exercises.Commands.DeleteExercise;
using Application.ModelHandling.Exercises.Commands.UpdateExercise;
using Application.ModelHandling.Exercises.Querries.GetAllExercises;
using Application.ModelHandling.Exercises.Querries.GetExerciseById;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/Exercises")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly ILogger<ExerciseController> _logger;
        private readonly IMediator _mediator;

        public ExerciseController(ILogger<ExerciseController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExercises()
        {
            _logger.LogInformation("Fetching all exercises");

            var result = await _mediator.Send(new GetAllExercisesQuery());

            if (result == null || !result.IsSuccess)
            {
                _logger.LogWarning("No exercises found or an error occurred");
                return NotFound(result);
            }

            _logger.LogInformation("{Count} exercises retrieved", result.Data?.Count ?? 0);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExerciseById(int id)
        {
            _logger.LogInformation("Fetching exercise with id {ExerciseId}", id);

            var result = await _mediator.Send(new GetExerciseByIdQuery { Id = id });

            if (result == null)
            {
                _logger.LogWarning("Exercise with id {ExerciseId} not found", id);
                return NotFound(new { message = $"Exercise with Id {id} does not exist" });
            }

            _logger.LogInformation("Exercise with id {ExerciseId} retrieved", id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExercise(CreateExerciseCommand command)
        {
            _logger.LogInformation("Creating new exercise: {ExerciseName}", command.Name);
            var result = await _mediator.Send(command);
            _logger.LogInformation("Exercise {ExerciseName} created successfully", command.Name);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, [FromBody] UpdateExerciseCommand command)
        {
            _logger.LogInformation("Updating exercise with id {ExerciseId}", id);

            if (id != command.Id)
            {
                _logger.LogWarning("Route id {RouteId} does not match body id {BodyId}", id, command.Id);
                return BadRequest(new { message = "Route id and body id must match" });
            }

            var result = await _mediator.Send(command);
            _logger.LogInformation("Exercise with id {ExerciseId} updated successfully", id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id, [FromBody] DeleteExerciseCommand command)
        {
            _logger.LogInformation("Deleting exercise with id {ExerciseId}", id);

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
                    _logger.LogWarning("Exercise with id {ExerciseId} not found or could not be deleted", id);
                    return NotFound(new { success = false, message = $"Exercise with Id {id} not found or could not be deleted." });
                }

                _logger.LogInformation("Exercise with id {ExerciseId} deleted successfully", id);
                return Ok(new { success = true, message = $"Exercise with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exercise with id {ExerciseId}", id);
                return StatusCode(500, new { success = false, message = $"Error deleting exercise: {ex.Message}" });
            }
        }
    }
}
