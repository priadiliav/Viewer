using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Interfaces.Repositories;

public interface IHeartbeatRepository 
{
	Task<Heartbeat?> GetByIdAsync(long id);
	Task CreateAsync(Heartbeat heartbeat);
	Task<IEnumerable<Heartbeat>> GetAllAsync();
}