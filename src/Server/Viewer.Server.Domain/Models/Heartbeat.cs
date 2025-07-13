using Viewer.Server.Domain.Interfaces;

namespace Viewer.Server.Domain.Models;

public class Heartbeat : ITrackable
{
	public long Id { get; set; }
	public Guid AgentId { get; set; }
	public DateTimeOffset? CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
	
	public virtual Agent Agent { get; set; } = null!;
}