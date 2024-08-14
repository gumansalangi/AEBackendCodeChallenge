using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Queryable;
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
        public async Task<ActionResult<List<User>>> GetUsers()
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
        public async Task<ActionResult<User>> CreateUser(User user)
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
                return CreatedAtAction(nameof(GetUsers), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> UpdateUserShips(UpdateUserQuery updateUserQuery)
        {
            try
            {
                if (updateUserQuery == null)
                    return BadRequest("User Id and Ship IDs are null or empty");

                var updatedUser = await _userService.UpdateUserShipsAsync(updateUserQuery.UsersId, updateUserQuery.ShipdId);
                if (updatedUser == null)
                    return NotFound($"User with ID " + updateUserQuery.UsersId + " not found or ships could not be updated");

                return Ok(new
                {
                    Name = updatedUser.Name,
                    Role = updatedUser.Role,
                    ShipId = updatedUser.Ships.FirstOrDefault().ShipId,
                    CurrentShipUserID = updatedUser.Ships.FirstOrDefault().UserId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating ships for user ID " + updateUserQuery.UsersId + ".");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
