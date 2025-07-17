using Viewer.Server.Application.Dtos.Heartbeat;

namespace Viewer.Server.Application.Interfaces.Services;

public interface IHeartbeatService
{
    Task<IEnumerable<HeartbeatDto>> GetAllAsync();
}
