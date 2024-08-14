using AEBackendCodeChallenge.Data;
using AEBackendCodeChallenge.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.Include(u => u.Ships).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                return new List<User>(); // or custom exception if needed
            }

        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Ships)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} was not found.");
                    return null;
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the user with ID {id}.");
                return null; // or throw a custom exception
            }
        }


        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new user.");
                return null; // or throw a custom exception
            }
        }



        public async Task<User> UpdateUserShipsAsync(int? userId, string shipIds)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Ships)
               .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {userId} was not found.");
                    return null;
                }
                var ships = await _context.Ships.Where(s => shipIds.Contains(s.ShipId)).ToListAsync();
                if (ships == null || ships.Count == 0)
                {
                    _logger.LogWarning($"No ships found for the provided IDs.");
                    return null;
                }

                user.Ships = ships;
                await _context.SaveChangesAsync();
                return user;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating ships for user ID {userId}.");
                return null; // or throw a custom exception
            }


           
        }
    }

}
