using Application.ModelHandling.Workout.Commands.CreateWorkout;
using Application.ModelHandling.Workout.Commands.DeleteWorkout;
using Application.ModelHandling.Workout.Commands.UpdateWorkout;
using Application.ModelHandling.Workout.Querries.GetAllWorkouts;
using Application.ModelHandling.Workout.Querries.GetWorkoutById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkoutController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkouts() =>
            Ok(await _mediator.Send(new GetAllWorkoutsQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutById(int id)
        {
            var result = await _mediator.Send(new GetWorkoutByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { message = $"Workout with Id {id} does not exist" });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkout(CreateWorkoutCommand command) =>
            Ok(await _mediator.Send(command));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkout(int id, [FromBody] UpdateWorkoutCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { message = "Route id and body id must match" });

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id, [FromBody] DeleteWorkoutCommand command)
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
                    return NotFound(new { success = false, message = $"Workout with Id {id} not found or could not be deleted." });
                }

                return Ok(new { success = true, message = $"Workout with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error deleting workout: {ex.Message}" });
            }
        }
    }

}
