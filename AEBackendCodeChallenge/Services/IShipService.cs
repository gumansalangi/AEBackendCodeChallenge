using AEBackendCodeChallenge.Models;

namespace AEBackendCodeChallenge.Services
{
    public interface IShipService
    {
        Task<List<Ship>> GetAllShipsAsync();
        Task<List<Ship>> GetUnassignedShipsAsync();
        Task<List<Ship>> GetShipsByUserIdAsync(int id);
        Task<Ship> GetShipByIdAsync(int id);
        Task<Ship> AddShipAsync(Ship ship);
        Task<Ship> UpdateShipVelocityAsync(string shipId, double velocity);
        Task<(Ship ship, Port closestPort, string estimatedArrivalTime)> GetClosestPortAsync(string shipId);

    }
}
