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
	
	public async Task DeleteAsync(long id)
	{
		var configuration = await abbDbContext.Configurations.FindAsync(id);
		if (configuration is not null)
			abbDbContext.Configurations.Remove(configuration);
	}

	public async Task<IEnumerable<Configuration>> GetAllAsync() =>
		await abbDbContext.Configurations
				.Include(x => x.Policies)
					.ThenInclude(x => x.Policy)
				.Include(x => x.Processes)
					.ThenInclude(x => x.Process)
				.Include(x => x.Agents)
				.OrderByDescending(x => x.CreatedAt)
				.ToListAsync();
}