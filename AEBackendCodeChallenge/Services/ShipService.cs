using AEBackendCodeChallenge.Data;
using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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

        public async Task<List<ShipWithUsersDto>> GetAllShipsAsync()
        {
            try
            {
                //Load all ship with or without user assign to the ship
                var shipsWithUsers = await _context.Ships
                    .Select(ship => new ShipWithUsersDto
                    {
                        ShipId = ship.Id,
                        Name = ship.Name,
                        ShipCode = ship.ShipCode,
                        Latitude = ship.Latitude,
                        Longitude = ship.Longitude,
                        Velocity = ship.Velocity,
                        Users = ship.UserShips
                                .Select(su => new UserDto
                                {
                                    UserId = su.UserId,
                                    UserName = su.User.Name,
                                    Role = su.User.Role
                                })
                                .ToList()
                    }).ToListAsync();

                return shipsWithUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all ships.");
                return null; // or throw a custom exception
            }
        }

        public async Task<List<ShipWithUsersDto>> GetUnassignedShipsAsync()
        {

            try
            {
                //Load all ship with or without user assign to the ship
                var shipsWithUsers = await _context.Ships
                    .Select(ship => new ShipWithUsersDto
                    {
                        ShipId = ship.Id,
                        Name = ship.Name,
                        ShipCode = ship.ShipCode,
                        Latitude = ship.Latitude,
                        Longitude = ship.Longitude,
                        Velocity = ship.Velocity,
                        Users = ship.UserShips
                                .Select(su => new UserDto
                                {
                                    UserId = su.UserId,
                                    UserName = su.User.Name,
                                    Role = su.User.Role
                                })
                                .ToList()
                    })
                    .Where(w => w.Users.Count <= 0)
                    .ToListAsync();

                return shipsWithUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all ships.");
                return null; // or throw a custom exception
            }
        }

        public async Task<ShipWithUsersDto> AssignShipToUserAsync(AssignUserToShipDto assignUserToShipDto)
        {
            //check from UserShip if not found then insert, if found do delete insert.
            var checkShipsUsers = await _context.UserShips.Where(u => u.ShipId == assignUserToShipDto.ShipId)
                .ToListAsync();

            if (checkShipsUsers.Count > 0)
            {
                //do remove based on shipid
                _context.UserShips.RemoveRange(checkShipsUsers);
            }

            //do insert
            foreach (var uId in assignUserToShipDto.UserIds)
            {
                UserShip userShipsData = new UserShip();
                userShipsData.ShipId = assignUserToShipDto.ShipId;
                userShipsData.UserId = uId;
                _context.UserShips.Add(userShipsData);
            }
            await _context.SaveChangesAsync();

            //Load all Ship with or without user for return value
            //Load all ship with or without user assign to the ship
            var shipsWithUsers = await _context.Ships
                .Select(ship => new ShipWithUsersDto
                {
                    ShipId = ship.Id,
                    Name = ship.Name,
                    ShipCode = ship.ShipCode,
                    Latitude = ship.Latitude,
                    Longitude = ship.Longitude,
                    Velocity = ship.Velocity,
                    Users = ship.UserShips
                            .Select(su => new UserDto
                            {
                                UserId = su.UserId,
                                UserName = su.User.Name,
                                Role = su.User.Role
                            })
                            .ToList()
                })
                .Where(w => w.ShipId == assignUserToShipDto.ShipId)
                .FirstOrDefaultAsync();

            return shipsWithUsers;
        }

        public async Task<IEnumerable<ShipWithUsersDto>> GetShipsByUserIdAsync(int userId)
        {
            try
            {
                //Load all ship with or without user assign to the ship
                var shipsWithUsers = await _context.Ships
                    .Select(ship => new ShipWithUsersDto
                    {
                        ShipId = ship.Id,
                        Name = ship.Name,
                        ShipCode = ship.ShipCode,
                        Latitude = ship.Latitude,
                        Longitude = ship.Longitude,
                        Velocity = ship.Velocity,
                        Users = ship.UserShips
                                .Select(su => new UserDto
                                {
                                    UserId = su.UserId,
                                    UserName = su.User.Name,
                                    Role = su.User.Role
                                })
                                .Where(u => u.UserId == userId)
                                .ToList()
                    })
                    .Where(w => w.Users.Count > 0)
                    .ToListAsync();

                return shipsWithUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all ships.");
                return null; // or throw a custom exception
            }
        }

        public async Task<Ship> GetShipByIdAsync(int id)
        {
            try
            {
                var ship = await _context.Ships.FirstOrDefaultAsync(s => s.Id == id);
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

        public async Task<ShipWithUsersDto> AddShipAsync(AddShipDto ship)
        {
            try
            {
                var shipUsers = new List<UserShip>();

                Ship newShip = new Ship
                {
                    Name = ship.Name,
                    ShipCode = ship.ShipCode,
                    Latitude = ship.Latitude,
                    Longitude = ship.Longitude,
                    Velocity = ship.Velocity
                };

                _context.Ships.Add(newShip);
                await _context.SaveChangesAsync();

                //IF the list of user is present from the input then continue to add the usership table
                if (ship.Users.Count > 0)
                {
                    foreach (var us in ship.Users)
                    {
                        UserShip uss = new UserShip();
                        uss.ShipId = newShip.Id;
                        uss.UserId = us.UserId;
                        _context.UserShips.Add(uss);
                    }
                }


                await _context.SaveChangesAsync();


                //reload created ship for return value
                var shipsWithUsers = await _context.Ships
                    .Select(ship => new ShipWithUsersDto
                    {
                        ShipId = ship.Id,
                        Name = ship.Name,
                        ShipCode = ship.ShipCode,
                        Latitude = ship.Latitude,
                        Longitude = ship.Longitude,
                        Velocity = ship.Velocity,
                        Users = ship.UserShips
                                .Select(su => new UserDto
                                {
                                    UserId = su.UserId,
                                    UserName = su.User.Name,
                                    Role = su.User.Role
                                })
                                .ToList()
                    })
                    .Where(w => w.ShipId == newShip.Id)
                    .FirstOrDefaultAsync();

                return shipsWithUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new ship.");
                return null; // or throw a custom exception
            }
        }


        public async Task<ShipWithUsersDto> UpdateShipVelocityAsync(int shipId, double velocity)
        {
            try
            {
                var ship = await _context.Ships
                    .FirstOrDefaultAsync(s => s.Id == shipId);
                if (ship == null)
                {
                    _logger.LogWarning($"Ship with ID {shipId} was not found.");
                    return null;
                }

                ship.Velocity = velocity;
                await _context.SaveChangesAsync();

                //Load all ship with or without user assign to the ship
                var shipsWithUsers = await _context.Ships
                    .Select(ship => new ShipWithUsersDto
                    {
                        ShipId = ship.Id,
                        Name = ship.Name,
                        ShipCode = ship.ShipCode,
                        Latitude = ship.Latitude,
                        Longitude = ship.Longitude,
                        Velocity = ship.Velocity,
                        Users = ship.UserShips
                                .Select(su => new UserDto
                                {
                                    UserId = su.UserId,
                                    UserName = su.User.Name,
                                    Role = su.User.Role
                                })
                                .ToList()
                    })
                    .Where(w => w.ShipId == shipId)
                    .FirstOrDefaultAsync();

                return shipsWithUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the velocity of ship ID " + shipId + ".");
                return null; // or throw a custom exception
            }
        }

        public async Task<ShipClosestPortDto> GetClosestPortAsync(GetClosestPortDto getClosestPort)
        {
            try
            {
                var ship = await _context.Ships
                    .Select(ship => new ShipWithUsersDto
                    {
                        ShipId = ship.Id,
                        Name = ship.Name,
                        ShipCode = ship.ShipCode,
                        Latitude = ship.Latitude,
                        Longitude = ship.Longitude,
                        Velocity = ship.Velocity,
                        Users = ship.UserShips
                                .Select(su => new UserDto
                                {
                                    UserId = su.UserId,
                                    UserName = su.User.Name,
                                    Role = su.User.Role
                                })
                                .ToList()
                    })
                    .Where(w => w.ShipId == getClosestPort.id)
                    .FirstOrDefaultAsync();
                if (ship == null)
                {
                    _logger.LogWarning($"Ship with ID " + getClosestPort.id + " was not found.");
                    return (null);
                }

                var ports = await _context.Ports.ToListAsync();
                if (ports == null || ports.Count == 0)
                {
                    _logger.LogWarning("No ports found.");
                    return (null);
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
                    return (null);
                }
                var estimatedArrivalTime = shortestDistance / ship.Velocity;

                //Populate return value
                string strEstimatedArrivalTime = FormatEstimatedTime(estimatedArrivalTime);

                ShipClosestPortInformationDto closestPortDto = new ShipClosestPortInformationDto
                {
                    PortInfo = closestPort,
                    EstimatedArrivalTime = strEstimatedArrivalTime,
                    EstimatedDistance = Math.Round((decimal)shortestDistance, 2).ToString() + " Nautical Miles"
                };


                ShipClosestPortDto shipClosestPortDto = new ShipClosestPortDto();
                shipClosestPortDto.shipDetailInfo = ship;
                shipClosestPortDto.PortInformation = closestPortDto;

                return (shipClosestPortDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while calculating the closest port for ship ID " + getClosestPort.id + ".");
                return (null); // or throw a custom exception
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
