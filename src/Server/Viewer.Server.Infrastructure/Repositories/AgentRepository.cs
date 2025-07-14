using Microsoft.EntityFrameworkCore;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Domain.Models;
using Viewer.Server.Infrastructure.Configs;

namespace Viewer.Server.Infrastructure.Repositories;

public class AgentRepository(AppDbContext appDbContext) : IAgentRepository
{
	public async Task<Agent?> GetByIdAsync(Guid id) =>
			await appDbContext.Agents
				.Include(x => x.Configuration)
				.FirstOrDefaultAsync(x => x.Id == id);

	public Task CreateAsync(Agent agent) =>
		appDbContext.Agents.AddAsync(agent).AsTask();

	public Task UpdateAsync(Agent agent)
	{
		appDbContext.Agents.Update(agent);
		return Task.CompletedTask;
	}

    public Task DeleteAsync(Agent agent)
    {
        appDbContext.Agents.Remove(agent);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Agent>> GetAllAsync() =>
		await appDbContext.Agents
				.Include(x => x.Configuration)
				.ToListAsync();
}
