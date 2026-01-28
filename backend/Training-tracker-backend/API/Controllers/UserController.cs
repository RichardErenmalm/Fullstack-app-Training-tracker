using Application.ModelHandling.Users.Commands.CreateUser;
using Application.ModelHandling.Users.Commands.DeleteUser;
using Application.ModelHandling.Users.Commands.UpdateUser;
using Application.ModelHandling.Users.Querries.GetAllUsers;
using Application.ModelHandling.Users.Querries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public UserController(ILogger<UserController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Fetching all users");

            var result = await _mediator.Send(new GetAllUsersQuery());

            if (result == null || !result.IsSuccess)
            {
                _logger.LogWarning("No users found or an error occurred");
                return NotFound(result);
            }

            _logger.LogInformation("{Count} users retrieved", result.Data?.Count ?? 0);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            _logger.LogInformation("Fetching user with id {UserId}", id);

            var result = await _mediator.Send(new GetUserByIdQuery { Id = id });

            if (result == null)
            {
                _logger.LogWarning("User with id {UserId} not found", id);
                return NotFound(new { message = $"User with Id {id} does not exist" });
            }

            _logger.LogInformation("User with id {UserId} retrieved", id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            _logger.LogInformation("Creating new user: {UserName}", command.Username);
            var result = await _mediator.Send(command);
            _logger.LogInformation("User {UserName} created successfully", command.Username);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command)
        {
            _logger.LogInformation("Updating user with id {UserId}", id);

            if (id != command.Id)
            {
                _logger.LogWarning("Route id {RouteId} does not match body id {BodyId}", id, command.Id);
                return BadRequest(new { message = "Route id and body id must match" });
            }

            var result = await _mediator.Send(command);
            _logger.LogInformation("User with id {UserId} updated successfully", id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromBody] DeleteUserCommand command)
        {
            _logger.LogInformation("Deleting user with id {UserId}", id);

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
                    _logger.LogWarning("User with id {UserId} not found or could not be deleted", id);
                    return NotFound(new { success = false, message = $"User with Id {id} not found or could not be deleted." });
                }

                _logger.LogInformation("User with id {UserId} deleted successfully", id);
                return Ok(new { success = true, message = $"User with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with id {UserId}", id);
                return StatusCode(500, new { success = false, message = $"Error deleting user: {ex.Message}" });
            }
        }
    }
}
