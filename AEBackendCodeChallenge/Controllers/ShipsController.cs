using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Dto;
using AEBackendCodeChallenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace AEBackendCodeChallenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShipsController : ControllerBase
    {
        private readonly IShipService _shipService;
        private readonly ILogger<ShipsController> _logger;

        public ShipsController(IShipService shipService, ILogger<ShipsController> logger)
        {
            _shipService = shipService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<ShipWithUsersDto>>> GetShips()
        {
            try
            {
                var ships = await _shipService.GetAllShipsAsync();
                return Ok(ships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching ships.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ShipWithUsersDto>>> GetUnassignedShips()
        {
            try
            {
                var unassignedShips = await _shipService.GetUnassignedShipsAsync();
                return Ok(unassignedShips);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching unassigned ships.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<ShipWithUsersDto>>> GetShipsBasedOnUserID(int id)
        {
            try
            {
                var shipBasedOnUserID = await _shipService.GetShipsByUserIdAsync(id);
                return Ok(shipBasedOnUserID);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching unassigned ships.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ShipWithUsersDto>> AssignShipToUser(AssignUserToShipDto assignUserToShipDto)
        {

            try
            {
                if (assignUserToShipDto == null)
                    return BadRequest("User Id and Ship IDs are null or empty");

                var updatedUser = await _shipService.AssignShipToUserAsync(assignUserToShipDto);
                if (updatedUser == null)
                    return NotFound($"User with ID " + assignUserToShipDto.ShipId + " not found or ships could not be updated");

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating ships for user ID " + assignUserToShipDto.ShipId + ".");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<ActionResult<ShipWithUsersDto>> CreateShip(AddShipDto ship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (ship == null)
                    return BadRequest("Ship data is null");

                var createdShip = await _shipService.AddShipAsync(ship);
                if (createdShip == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ship could not be added");

                return CreatedAtAction(nameof(GetShips), new
                {
                    id = createdShip.ShipId
                }, createdShip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a ship.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ShipWithUsersDto>> UpdateVelocity(UpdateShipVelocityDto updateVelocityQuery)
        {
            try
            {
                if (updateVelocityQuery.Velocity <= 0)
                    return BadRequest("Velocity must be greater than zero");

                var updatedShip = await _shipService.UpdateShipVelocityAsync(updateVelocityQuery.ShipId, updateVelocityQuery.Velocity);
                if (updatedShip == null)
                    return NotFound("Ship with ID {id} not found or velocity could not be updated");

                return Ok(updatedShip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the velocity for ship ID {updateVelocityQuery.ShipId}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ShipClosestPortDto>> GetClosestPort(GetClosestPortDto getClosestPort)
        {
            try
            {
                var result = await _shipService.GetClosestPortAsync(getClosestPort);
                if (result.PortInformation == null)
                    return NotFound("Ship with ID " + getClosestPort.id + " not found or no closest port could be determined");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while calculating the closest port for ship ID "+ getClosestPort.id+".");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}
