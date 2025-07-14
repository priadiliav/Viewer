using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Interfaces.Repositories;

public interface IAgentRepository
{
	Task<Agent?> GetByIdAsync(Guid id);
	Task CreateAsync(Agent agent);
	Task UpdateAsync(Agent agent);
    Task DeleteAsync(Agent id);
	Task<IEnumerable<Agent>> GetAllAsync();
}
