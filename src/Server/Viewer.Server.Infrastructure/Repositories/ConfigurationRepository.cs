using Microsoft.EntityFrameworkCore;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Domain.Models;
using Viewer.Server.Infrastructure.Configs;

namespace Viewer.Server.Infrastructure.Repositories;

public class ConfigurationRepository(AppDbContext abbDbContext) : IConfigurationRepository
{
	public async Task<Configuration?> GetByIdAsync(long id) =>
		await abbDbContext.Configurations
			.Include(x => x.Policies)
				.ThenInclude(x => x.Policy)
			.Include(x => x.Processes)
				.ThenInclude(x => x.Process)
			.Include(x => x.Agents)
			.FirstOrDefaultAsync(x => x.Id == id);
	 
	public Task CreateAsync(Configuration configuration) =>
		abbDbContext.Configurations.AddAsync(configuration).AsTask();

	public Task UpdateAsync(Configuration configuration) 
	{
		abbDbContext.Configurations.Update(configuration);
		return Task.CompletedTask;
	}
	
	public Task DeleteAsync(Configuration configuration)
	{
        abbDbContext.Configurations.Remove(configuration);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Configuration>> GetByPolicyIdAsync(long policyId) =>
        await abbDbContext.Configurations
                .Where(x => x.Policies.Any(p => p.PolicyId == policyId))
                .ToListAsync();

    public async Task<IEnumerable<Configuration>> GetByProcessIdAsync(long processId) =>
        await abbDbContext.Configurations
                .Where(x => x.Processes.Any(p => p.ProcessId == processId))
                .ToListAsync();

    public async Task<bool> ExistsByPolicyIdAsync(long policyId)
        => await abbDbContext.Configurations
            .AnyAsync(x => x.Policies.Any(p => p.PolicyId == policyId));

    public async Task<bool> ExistsByProcessIdAsync(long processId)
       => await abbDbContext.Configurations
            .AnyAsync(x => x.Processes.Any(p => p.ProcessId == processId));

    public async Task<IEnumerable<Configuration>> GetAllAsync() =>
		await abbDbContext.Configurations
				.Include(x => x.Policies)
				.Include(x => x.Processes)
				.Include(x => x.Agents)
				.OrderByDescending(x => x.CreatedAt)
				.ToListAsync();
}
