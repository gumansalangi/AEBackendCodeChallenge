using AEBackendCodeChallenge.Data;
using AEBackendCodeChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace AEBackendCodeChallenge.Services
{
    public class ShipService : IShipService
    {
        private readonly ShipDbContext _context;
        private readonly ILogger<ShipService> _logger;

        public ShipService(ShipDbContext context, ILogger<ShipService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Ship>> GetAllShipsAsync()
        {
            try
            {
                return await _context.Ships.Include(s => s.User).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all ships.");
                return new List<Ship>(); // or throw a custom exception
            }
        }

        public async Task<List<Ship>> GetUnassignedShipsAsync()
        {
            try
            {
                return await _context.Ships.Where(s => s.UserId == null).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching unassigned ships.");
                return new List<Ship>(); // or throw a custom exception
            }
        }

        public async Task<List<Ship>> GetShipsByUserIdAsync(int id)
        {
            try
            {
                var ship = await _context.Ships.Where(s => s.UserId == id).ToListAsync();
                if (ship == null)
                {
                    _logger.LogWarning($"Ship with ID " + id + " was not found.");
                    return null;
                }
                return ship;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the ship with ID {id}.");
                return null; // or throw a custom exception
            }
        }


        public async Task<Ship> GetShipByIdAsync(int id)
        {
            try
            {
                var ship = await  _context.Ships.FirstOrDefaultAsync(s => s.Id == id);
                if (ship == null)
                {
                    _logger.LogWarning($"Ship with ID "+ id + " was not found.");
                    return null;
                }
                return ship;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the ship with ID {id}.");
                return null; // or throw a custom exception
            }
        }


        public async Task<Ship> AddShipAsync(Ship ship)
        {
            try
            {
                _context.Ships.Add(ship);
                await _context.SaveChangesAsync();
                return ship;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new ship.");
                return null; // or throw a custom exception
            }
        }


        public async Task<Ship> UpdateShipVelocityAsync(string shipId, double velocity)
        {
            try
            {
                var ship = await _context.Ships
                    .FirstOrDefaultAsync(s => s.ShipId == shipId);
                if (ship == null)
                {
                    _logger.LogWarning($"Ship with ID {shipId} was not found.");
                    return null;
                }

                ship.Velocity = velocity;
                await _context.SaveChangesAsync();
                return ship;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the velocity of ship ID {shipId}.");
                return null; // or throw a custom exception
            }
        }

        public async Task<(Ship ship, Port closestPort, string estimatedArrivalTime)> GetClosestPortAsync(string shipId)
        {
            try
            {
                var ship = await _context.Ships.FirstOrDefaultAsync(s => s.ShipId == shipId);
                if (ship == null)
                {
                    _logger.LogWarning($"Ship with ID " + shipId + " was not found.");
                    return (null, null, "0");
                }

                var ports = await _context.Ports.ToListAsync();
                if (ports == null || ports.Count == 0)
                {
                    _logger.LogWarning("No ports found.");
                    return (null, null, "0");
                }

                Port closestPort = null;
                double shortestDistance = double.MaxValue;

                foreach (var port in ports)
                {
                    var distance = CalculateDistance(ship.Latitude, ship.Longitude, port.Latitude, port.Longitude);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        closestPort = port;
                    }
                }

                if (closestPort == null)
                {
                    _logger.LogWarning("No closest port could be determined.");
                    return (null, null, "0");
                }

                var estimatedArrivalTime = shortestDistance / ship.Velocity;
                return (ship, closestPort, FormatEstimatedTime(estimatedArrivalTime));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while calculating the closest port for ship ID " + shipId + ".");
                return (null, null, "0"); // or throw a custom exception
            }
        }

        private string FormatEstimatedTime(double estimatedTime)
        {
            int days = Convert.ToInt32(estimatedTime) / 24;
            int hours = Convert.ToInt32(estimatedTime) % 24;

            string strEstimatedTime = "Estimated Arrival Time " + days + " days and " +
                hours + " hours.";
            return strEstimatedTime;
        }

        private double CalculateDistance(double latitude1, double longitude1,
            double latitude2, double longitude2)
        {
            try
            {
                // Haversine formula to calculate the distance between two points on the Earth's surface
                const double earthRadius = 6371e3; //earth radius in meter
                var phi1 = latitude1 * (Math.PI / 100);
                var phi2 = latitude2 * (Math.PI / 100);
                var deltaPhi = (latitude2 - latitude1) * (Math.PI / 100);
                var deltaLambda = (longitude2 - longitude1) * (Math.PI / 100);

                var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                    Math.Cos(phi1) * Math.Cos(phi2) *
                    Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                var distance = earthRadius * c; // in meters
                return distance / 1000; // in kilometers
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the distance.");
                return 0; // return zero if the calculation fails
            }
        }
    }
}
