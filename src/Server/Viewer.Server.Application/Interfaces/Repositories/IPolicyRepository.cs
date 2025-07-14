using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Interfaces.Repositories;

public interface IPolicyRepository
{
	Task<Policy?> GetByIdAsync(long id);
	Task CreateAsync(Policy policy);
	Task UpdateAsync(Policy policy);
	Task DeleteAsync(Policy policy);
	Task<IEnumerable<Policy>> GetAllAsync();
}
