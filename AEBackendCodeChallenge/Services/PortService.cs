using AEBackendCodeChallenge.Data;
using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace AEBackendCodeChallenge.Services
{
    public class PortService : IPortService
    {
        private readonly ShipDbContext _context;
        private readonly ILogger<PortService> _logger;

        public PortService(ShipDbContext context, ILogger<PortService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Port>> GetAllPortsAsync()
        {
            try
            {
                return await _context.Ports.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all ports.");
                throw new Exception("Unable to retrieve ports at this time.");
            }
        }

        public async Task<Port> AddPortAsync(Port port)
        {
            try
            {
                await _context.Ports.AddAsync(port);
                await _context.SaveChangesAsync();  
                return port;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new port.");
                throw new Exception("Unable to add port at this time.");
            }
           
        }

        public async Task<Port> UpdatePortAsync(PortDto updatedPort)
        {
            try
            {
                var existingPort = await _context.Ports.FindAsync(updatedPort.Id);
                if (existingPort != null)
                {
                    throw new KeyNotFoundException("Port is not found");
                }

                existingPort.Name = updatedPort.Name;
                existingPort.Latitude = updatedPort.Latitude;
                existingPort.Longitude = updatedPort.Longitude;

                _context.Ports.Update(existingPort);
                await _context.SaveChangesAsync();
                return existingPort;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating port.");
                throw new Exception("Unable to update port at this time.");
            }
        }
    }
}
