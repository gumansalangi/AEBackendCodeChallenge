using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Queryable;
using AEBackendCodeChallenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AEBackendCodeChallenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PortsController : ControllerBase
    {
        private readonly IPortService _portService;
        private readonly ILogger<PortsController> _logger;

        public PortsController(IPortService portService, ILogger<PortsController> logger)
        {
            _portService = portService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Port>>> GetAllPorts()
        {
            try
            {
                var ports = await _portService.GetAllPortsAsync();
                return Ok(ports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve ports.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Port>> AddPort([FromBody] Port port)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (port == null)
                return BadRequest("Port data is null");

            try
            {
                var createdPort = await _portService.AddPortAsync(port);
                return CreatedAtAction(nameof(GetAllPorts), new
                {
                    id = createdPort.Id
                }, createdPort);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add port.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Port>> UpdatePorts (UpdatePortQuery updatedPort)
        {
            if (updatedPort == null)
                return BadRequest("Invalid dort data");

            try
            {
                var port = await _portService.UpdatePortAsync(updatedPort);
                return Ok(port);    
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Port not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update port.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }



    }
}
