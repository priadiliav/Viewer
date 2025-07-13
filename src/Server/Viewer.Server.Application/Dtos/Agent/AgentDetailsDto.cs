using Viewer.Server.Application.Dtos.Heartbeat;

namespace Viewer.Server.Application.Dtos.Agent;

public class AgentDetailsDto
{
	public Guid Id { get; set; } 
	public string Name { get; set; } = string.Empty;
	public bool IsConnected { get; set; } = false;

	public DateTimeOffset? CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
	
	public ConfigurationDto Configuration { get; set; } = new();
}