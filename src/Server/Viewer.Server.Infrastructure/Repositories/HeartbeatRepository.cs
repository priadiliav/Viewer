using Microsoft.EntityFrameworkCore;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Domain.Models;
using Viewer.Server.Infrastructure.Configs;

namespace Viewer.Server.Infrastructure.Repositories;

public class HeartbeatRepository(AppDbContext context) : IHeartbeatRepository
{
	public Task<Heartbeat?> GetByIdAsync(long id)
		=> context.Heartbeats.FindAsync(id).AsTask();

	public Task CreateAsync(Heartbeat heartbeat) 
		=> context.Heartbeats.AddAsync(heartbeat).AsTask();

	public async Task<IEnumerable<Heartbeat>> GetAllAsync()
		=> await context.Heartbeats
				.Include(x => x.Agent)
				.ToListAsync();
}