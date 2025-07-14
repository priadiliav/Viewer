using Microsoft.EntityFrameworkCore;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Domain.Models;
using Viewer.Server.Infrastructure.Configs;

namespace Viewer.Server.Infrastructure.Repositories;

public class PolicyRepository(AppDbContext context) : IPolicyRepository
{
	public Task<Policy?> GetByIdAsync(long id) =>
		context.Policies.FindAsync(id).AsTask();

	public Task CreateAsync(Policy policy) =>
		context.Policies.AddAsync(policy).AsTask();

	public Task UpdateAsync(Policy policy)
	{
		context.Policies.Update(policy);
		return Task.CompletedTask;
	}

    public Task DeleteAsync(Policy policy)
    {
        context.Policies.Remove(policy);
        return Task.CompletedTask;
    }

	public async Task<IEnumerable<Policy>> GetAllAsync() =>
		await context.Policies.ToListAsync();
}
