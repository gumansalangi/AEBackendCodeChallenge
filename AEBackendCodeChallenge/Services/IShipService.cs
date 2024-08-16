using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Dto;

namespace AEBackendCodeChallenge.Services
{
    public interface IShipService
    {
        Task<List<ShipWithUsersDto>> GetAllShipsAsync();
        Task<List<ShipWithUsersDto>> GetUnassignedShipsAsync();
        Task<IEnumerable<ShipWithUsersDto>> GetShipsByUserIdAsync(int id);
        Task<Ship> GetShipByIdAsync(int id);
        Task<ShipWithUsersDto> AddShipAsync(AddShipDto ship);
        Task<ShipWithUsersDto> UpdateShipVelocityAsync(int shipId, double velocity);
        Task<ShipClosestPortDto> GetClosestPortAsync(GetClosestPortDto getClosestPortDto);
        Task<ShipWithUsersDto> AssignShipToUserAsync(AssignUserToShipDto assignUserToShipDto);

    }
}
