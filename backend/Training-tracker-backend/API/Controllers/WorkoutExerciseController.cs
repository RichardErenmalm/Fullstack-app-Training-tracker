using Application.ModelHandling.WorkoutExercise.Commands.CreateWorkoutExercise;
using Application.ModelHandling.WorkoutExercise.Commands.DeleteWorkoutExercise;
using Application.ModelHandling.WorkoutExercise.Commands.UpdateWorkoutExercise;
using Application.ModelHandling.WorkoutExercise.Querries.GetAllWorkoutExercises;
using Application.ModelHandling.WorkoutExercise.Querries.GetWorkoutExerciseById;
using Application.ModelHandling.WorkoutExercise.Querries.GetWorkoutExercisesByWorkoutId;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/WorkoutExercises")]
    [ApiController]
    public class WorkoutExerciseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkoutExerciseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutExercises() =>
            Ok(await _mediator.Send(new GetAllWorkoutExercisesQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutExerciseById(int id)
        {
            var result = await _mediator.Send(new GetWorkoutExerciseByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { message = $"WorkoutExercise with Id {id} does not exist" });

            return Ok(result);
        }

        [HttpGet("workout/{workoutId}")]
        public async Task<IActionResult> GetWorkoutExercisesByWorkoutId(int workoutId)
        {
            var result = await _mediator.Send(
                new GetWorkoutExercisesByWorkoutIdQuery(workoutId)
            );

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateWorkoutExercise(CreateWorkoutExerciseCommand command) =>
            Ok(await _mediator.Send(command));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkoutExercise(int id, [FromBody] UpdateWorkoutExerciseCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { message = "Route id and body id must match" });

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutExercise(int id, [FromBody] DeleteWorkoutExerciseCommand command)
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
                    return NotFound(new { success = false, message = $"WorkoutExercise with Id {id} not found or could not be deleted." });
                }

                return Ok(new { success = true, message = $"WorkoutExercise with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error deleting WorkoutExercise: {ex.Message}" });
            }
        }
    }

}
