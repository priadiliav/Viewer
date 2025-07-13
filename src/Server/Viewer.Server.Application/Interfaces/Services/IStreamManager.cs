namespace Viewer.Server.Application.Interfaces.Services;

public interface IStreamManager
{
	List<Guid> GetConnectedAgentIds();
	bool IsAgentConnected(Guid agentId);
}