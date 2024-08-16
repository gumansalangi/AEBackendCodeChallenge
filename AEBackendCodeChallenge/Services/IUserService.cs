using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Dto;

namespace AEBackendCodeChallenge.Services
{
    public interface IUserService
    {
        Task<List<UserWithShipsDto>> GetAllUsersAsync();
        Task<UserWithShipsDto> GetUserByIdAsync(int id);
        Task<UserWithShipsDto> AddUserAsync(AddUserDto user);
        Task<UserWithShipsDto> UpdateUserShipsAsync(AssignShipToUserDto assignShipToUser);
    }
}
