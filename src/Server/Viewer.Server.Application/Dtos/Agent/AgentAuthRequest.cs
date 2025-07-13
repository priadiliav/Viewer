namespace Viewer.Server.Application.Dtos.Agent;

public class AgentAuthRequest
{
	public Guid AgentId { get; set; } = Guid.Empty;
	public string AgentSecret { get; set; } = string.Empty;
}