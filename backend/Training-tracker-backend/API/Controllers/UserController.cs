using Application.ModelHandling.Users.Commands.CreateUser;
using Application.ModelHandling.Users.Commands.DeleteUser;
using Application.ModelHandling.Users.Commands.UpdateUser;
using Application.ModelHandling.Users.Querries.GetAllUsers;
using Application.ModelHandling.Users.Querries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers() =>
            Ok(await _mediator.Send(new GetAllUsersQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { message = $"User with Id {id} does not exist" });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command) =>
          Ok(await _mediator.Send(command));


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { message = "Route id and body id must match" });

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromBody] DeleteUserCommand command)
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
                    return NotFound(new { success = false, message = $"User with Id {id} not found or could not be deleted." });
                }

                return Ok(new { success = true, message = $"User with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error deleting user: {ex.Message}" });
            }
        }

    }
}
