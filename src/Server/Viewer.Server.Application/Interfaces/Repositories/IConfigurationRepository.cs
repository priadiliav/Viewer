using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Interfaces.Repositories;

public interface IConfigurationRepository
{
	Task<Configuration?> GetByIdAsync(long id);
	Task CreateAsync(Configuration configuration);
	Task UpdateAsync(Configuration configuration);
	Task DeleteAsync(long id);
	Task<IEnumerable<Configuration>> GetAllAsync();
}