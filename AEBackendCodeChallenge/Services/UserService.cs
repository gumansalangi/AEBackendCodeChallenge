using AEBackendCodeChallenge.Data;
using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AEBackendCodeChallenge.Services
{
    public class UserService : IUserService
    {
        private readonly ShipDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ShipDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public UserService(ShipDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserWithShipsDto>> GetAllUsersAsync()
        {
            try
            {
                //Load all user with or without ship assign to the user
                var usersWithShips = await _context.Users
                       .Select(user => new UserWithShipsDto
                       {
                           UserId = user.Id,
                           UserName = user.Name,
                           Role = user.Role,
                           Ships = user.UserShips
                                       .Select(us => new ShipDto
                                       {
                                           ShipId = us.ShipId,
                                           Name = us.Ship.Name,
                                           Latitude = us.Ship.Latitude,
                                           Longitude = us.Ship.Longitude,
                                           Velocity = us.Ship.Velocity
                                       })
                                       .ToList()
                       })
                       .ToListAsync();

                return usersWithShips;
                //var userWithShips = await _context.Users
                //    .GroupJoin(
                //    _context.UserShips,
                //    user => user.Id,
                //    userShip => userShip.UserId,
                //    (user, userShips) => new
                //    {
                //        User = user,
                //        UserShips = userShips
                //    })
                //    .SelectMany(
                //    userWithShips => userWithShips.UserShips.DefaultIfEmpty(),
                //    (userWithShips, userShip) => new UserWithShipsDto
                //    {
                //        UserId = userWithShips.User.Id,
                //        UserName = userWithShips.User.Name,
                //        Role = userWithShips.User.Role,
                //        Ships = userWithShips.UserShips.Select(us => new ShipDto
                //        {
                //            ShipId = us.ShipId,
                //            Name = us.Ship.Name,
                //            Latitude = us.Ship.Latitude,
                //            Longitude = us.Ship.Longitude,
                //            Velocity = us.Ship.Velocity,
                //        }).ToList()
                //    }).ToListAsync();
                //return userWithShips;
                //return await _context.Users.Include(u => u.UserShips).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                return null; // or custom exception if needed
            }

        }

        public async Task<UserWithShipsDto> GetUserByIdAsync(int id)
        {
            try
            {
                var usersWithShips = await _context.Users
                      .Select(user => new UserWithShipsDto
                      {
                          UserId = user.Id,
                          UserName = user.Name,
                          Role = user.Role,
                          Ships = user.UserShips
                                      .Select(us => new ShipDto
                                      {
                                          ShipId = us.ShipId,
                                          Name = us.Ship.Name,
                                          Latitude = us.Ship.Latitude,
                                          Longitude = us.Ship.Longitude,
                                          Velocity = us.Ship.Velocity
                                      })
                                      .ToList()
                      })
                      .Where(u => u.UserId== id)
                      .FirstOrDefaultAsync();

                if (usersWithShips == null)
                {
                    _logger.LogWarning($"User with ID "+ id + " was not found.");
                    return null;
                }
                return usersWithShips;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the user with ID {id}.");
                return null; // or throw a custom exception
            }
        }


        public async Task<UserWithShipsDto> AddUserAsync(AddUserDto addUser)
        {
            try
            {
                var userShips = new List<UserShip>();
                

                User newUser = new User
                {
                    Name = addUser.UserName,
                    Role= addUser.Role,
                    UserShips = userShips
                };


                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                //IF the list of ship is present from the input then continue to add the usership table

                if (addUser.Ships.Count > 0)
                {
                    foreach (var sh in addUser.Ships)
                    {
                        UserShip uss = new UserShip();
                        uss.ShipId = sh.ShipId;
                        uss.UserId = newUser.Id;
                        _context.UserShips.Add(uss);
                    }
                }
               
                await _context.SaveChangesAsync();

                //reload created user for return value
                var returnedNewUser = await _context.Users
                      .Select(user => new UserWithShipsDto
                      {
                          UserId = user.Id,
                          UserName = user.Name,
                          Role = user.Role,
                          Ships = user.UserShips
                                      .Select(us => new ShipDto
                                      {
                                          ShipId = us.ShipId,
                                          Name = us.Ship.Name,
                                          Latitude = us.Ship.Latitude,
                                          Longitude = us.Ship.Longitude,
                                          Velocity = us.Ship.Velocity
                                      })
                                      .ToList()
                      })
                      .Where(u => u.UserId == newUser.Id)
                      .FirstOrDefaultAsync();


                return returnedNewUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new user.");
                return null; // or throw a custom exception
            }
        }



        public async Task<UserWithShipsDto> UpdateUserShipsAsync(AssignShipToUserDto assignShipToUser)
        {
            try
            {

                //check from UserShip if not found then insert, if found do delete insert.
                var checkUserShips = await _context.UserShips.Where(s => s.UserId == assignShipToUser.UsersId)
                    .ToListAsync();

                if (checkUserShips.Count > 0)
                {
                    //do remove based on userID
                    _context.UserShips.RemoveRange(checkUserShips);
                }
                //do insert
                foreach (var sId in assignShipToUser.ShipIds)
                {
                    UserShip userShipsData = new UserShip();
                    userShipsData.UserId = assignShipToUser.UsersId;
                    userShipsData.ShipId = sId;
                    _context.UserShips.Add(userShipsData);
                }
                await _context.SaveChangesAsync();


                //Load all user with or without ship assign to the user
                var usersWithShips = await _context.Users
                       .Select(user => new UserWithShipsDto
                       {
                           UserId = user.Id,
                           UserName = user.Name,
                           Role = user.Role,
                           Ships = user.UserShips
                                       .Select(us => new ShipDto
                                       {
                                           ShipId = us.ShipId,
                                           Name = us.Ship.Name,
                                           Latitude = us.Ship.Latitude,
                                           Longitude = us.Ship.Longitude,
                                           Velocity = us.Ship.Velocity
                                       })
                                       .ToList()
                       })
                       .Where(s => s.UserId == assignShipToUser.UsersId)
                       .FirstOrDefaultAsync();

                return usersWithShips;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating ships for user ID " + assignShipToUser.UsersId + ".");
                return null; // or throw a custom exception
            }



        }
    }

}
