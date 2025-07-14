using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Interfaces.Repositories;

public interface IConfigurationRepository
{
	Task<Configuration?> GetByIdAsync(long id);
	Task CreateAsync(Configuration configuration);
	Task UpdateAsync(Configuration configuration);
	Task DeleteAsync(Configuration configuration);
	Task<IEnumerable<Configuration>> GetAllAsync();
    Task<IEnumerable<Configuration>> GetByPolicyIdAsync(long policyId);
    Task<IEnumerable<Configuration>> GetByProcessIdAsync(long processId);
    Task<bool> ExistsByPolicyIdAsync(long policyId);
    Task<bool> ExistsByProcessIdAsync(long processId);
}
