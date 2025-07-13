namespace Viewer.Server.Domain.Models;

public class PolicyInConfiguration : ITrackable
{
	public long ConfigurationId { get; set; }
	public long PolicyId { get; set; }

	public DateTimeOffset? CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
	
	public virtual Configuration Configuration { get; set; } = null!;
	public virtual Policy Policy { get; set; } = null!;
}