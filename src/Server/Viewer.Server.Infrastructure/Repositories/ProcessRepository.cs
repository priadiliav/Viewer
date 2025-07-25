using Microsoft.EntityFrameworkCore;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Domain.Models;
using Viewer.Server.Infrastructure.Configs;

namespace Viewer.Server.Infrastructure.Repositories;

public class ProcessRepository(AppDbContext context) : IProcessRepository
{
	public Task<Process?> GetByIdAsync(long id) =>
			context.Proceses.FindAsync(id).AsTask();

	public Task CreateAsync(Process process) =>
			context.Proceses.AddAsync(process).AsTask();

	public Task UpdateAsync(Process process)
	{
		context.Proceses.Update(process);
		return Task.CompletedTask;
	}

	public Task DeleteAsync(Process process)
	{
        context.Proceses.Remove(process);
        return Task.CompletedTask;
    }

	public async Task<IEnumerable<Process>> GetAllAsync() =>
			await context.Proceses.ToListAsync();
}
