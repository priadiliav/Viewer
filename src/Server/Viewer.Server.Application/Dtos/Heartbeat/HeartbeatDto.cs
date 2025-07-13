namespace Viewer.Server.Application.Dtos.Heartbeat;

public class HeartbeatDto
{
	public long Id { get; set; }
	public Guid AgentId { get; set; }
	
	public DateTimeOffset? CreatedAt { get; set; }
}