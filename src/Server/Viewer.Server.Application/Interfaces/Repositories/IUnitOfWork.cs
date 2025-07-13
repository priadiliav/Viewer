namespace Viewer.Server.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
	IAgentRepository Agents { get; }
	IConfigurationRepository Configurations { get; }
	IProcessRepository Processes { get; }
	IPolicyRepository Policies { get; }
	IHeartbeatRepository Heartbeats { get; }
	Task<int> SaveChangesAsync();
}
