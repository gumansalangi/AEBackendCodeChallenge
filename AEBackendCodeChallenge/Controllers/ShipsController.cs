using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Queryable;
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
        public async Task<ActionResult<List<Ship>>> GetShips()
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
        public async Task<ActionResult<List<Ship>>> GetUnassignedShips()
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
        public async Task<ActionResult<List<Ship>>> GetShipsBasedOnUserID(int id)
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
        public async Task<ActionResult<Ship>> CreateShip(Ship ship)
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
                    id = createdShip.Id
                }, createdShip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a ship.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Ship>> UpdateVelocity(UpdateVelocityQuery updateVelocityQuery)
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
        public async Task<ActionResult<object>> GetClosestPort(string shipId)
        {
            try
            {
                var result = await _shipService.GetClosestPortAsync(shipId);
                if (result.closestPort == null)
                    return NotFound("Ship with ID " + shipId + " not found or no closest port could be determined");

                return Ok(new 
                {
                    ShipInfo    = result.ship, 
                    ClosestPort = result.closestPort,
                    EstimatedArrivalTime = result.estimatedArrivalTime,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while calculating the closest port for ship ID {shipId}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}
