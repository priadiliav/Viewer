namespace Viewer.Server.Domain.Models;

public class Agent : ITrackable
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public long ConfigurationId { get; set; }
	
	public DateTimeOffset? CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
	
	public virtual Configuration Configuration { get; set; } = null!;
}