using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Infrastructure.Configs;

namespace Viewer.Server.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly AppDbContext _context;
	public IPolicyRepository Policies { get; }
	public IHeartbeatRepository Heartbeats { get; }
	public IConfigurationRepository Configurations { get; }
	public IAgentRepository Agents { get; }
	public IProcessRepository Processes { get; }

	public UnitOfWork(AppDbContext context, 
		IPolicyRepository policyRepository, 
		IConfigurationRepository configurationRepository,
		IAgentRepository agentRepository,
		IProcessRepository processRepository,
		IHeartbeatRepository heartbeatRepository)
	{
		_context = context;
		Policies = policyRepository;
		Configurations = configurationRepository;
		Agents = agentRepository;
		Processes = processRepository;
		Heartbeats = heartbeatRepository;
	}
	
	// todo: make transactional in feature 
	public async Task<int> SaveChangesAsync()
	{
		// using var tx = await _dbContext.Database.BeginTransactionAsync();
		return await _context.SaveChangesAsync();
		// await tx.CommitAsync();
	}

	public void Dispose()
	{
		_context.Dispose();
	}
}
