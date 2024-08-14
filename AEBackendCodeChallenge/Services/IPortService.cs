using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Queryable;

namespace AEBackendCodeChallenge.Services
{
    public interface IPortService
    {
        Task<List<Port>> GetAllPortsAsync();
        Task<Port> AddPortAsync(Port port);
        Task<Port> UpdatePortAsync(UpdatePortQuery updatedPort);
    }
}
