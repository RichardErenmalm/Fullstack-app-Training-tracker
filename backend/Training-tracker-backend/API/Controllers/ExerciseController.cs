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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ExerciseController(IMediator mediator) 
        { 
            _mediator = mediator;  
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExercises() =>
            Ok(await _mediator.Send(new GetAllExercisesQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExerciseById(int id)
        {
            var result = await _mediator.Send(new GetExerciseByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { message = $"Exercise with Id {id} does not exist" });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExercise(CreateExerciseCommand command) =>
          Ok(await _mediator.Send(command));


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, [FromBody] UpdateExerciseCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { message = "Route id and body id must match" });

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id, [FromBody] DeleteExerciseCommand command)
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
                    return NotFound(new { success = false, message = $"Exercise with Id {id} not found or could not be deleted." });
                }

                return Ok(new { success = true, message = $"Exercise with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error deleting exercise: {ex.Message}" });
            }
        }


    }
}
