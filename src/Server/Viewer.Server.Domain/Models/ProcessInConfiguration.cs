namespace Viewer.Server.Domain.Models;

public class ProcessInConfiguration : ITrackable
{
	public long ConfigurationId { get; set; }
	public long ProcessId { get; set; }
	
	public DateTimeOffset? CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
	
	public virtual Configuration Configuration { get; set; } = null!;
	public virtual Process Process { get; set; } = null!;
}