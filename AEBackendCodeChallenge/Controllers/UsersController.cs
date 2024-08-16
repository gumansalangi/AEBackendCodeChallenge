using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Dto;
using AEBackendCodeChallenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AEBackendCodeChallenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserWithShipsDto>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserWithShipsDto>> CreateUser(AddUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (user == null)
                    return BadRequest("User data is null");

                var createdUser = await _userService.AddUserAsync(user);
                if (createdUser == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "User could not be created");

                //user successfully created
                return CreatedAtAction(nameof(GetUsers), new { id = createdUser.UserId }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a user.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString() + " Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserWithShipsDto>> AssignUserToShips(AssignShipToUserDto assignShipToUser)
        {
            try
            {
                if (assignShipToUser == null)
                    return BadRequest("User Id and Ship IDs are null or empty");

                var updatedUser = await _userService.UpdateUserShipsAsync(assignShipToUser);
                if (updatedUser == null)
                    return NotFound($"User with ID " + assignShipToUser.UsersId + " not found or ships could not be updated");

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating ships for user ID " + assignShipToUser.UsersId + ".");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
