namespace Viewer.Server.Application.Dtos.Agent;

public class AgentDto
{
	public Guid Id { get; set; }
	
	public string Name { get; set; } = string.Empty;
	public long ConfigurationId { get; set; }
	public bool IsConnected { get; set; } = false;
}