using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Interfaces.Repositories;

public interface IProcessRepository
{
	Task<Process?> GetByIdAsync(long id);
	Task CreateAsync(Process process);
	Task UpdateAsync(Process process);
	Task DeleteAsync(long id);
	Task<IEnumerable<Process>> GetAllAsync();
}