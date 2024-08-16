using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Dto;

namespace AEBackendCodeChallenge.Services
{
    public interface IPortService
    {
        Task<List<Port>> GetAllPortsAsync();
        Task<Port> AddPortAsync(Port port);
        Task<Port> UpdatePortAsync(PortDto updatedPort);
    }
}
