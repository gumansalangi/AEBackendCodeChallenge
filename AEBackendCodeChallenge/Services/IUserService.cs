using AEBackendCodeChallenge.Models;

namespace AEBackendCodeChallenge.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserShipsAsync(int userId, string shipIds);
    }
}
