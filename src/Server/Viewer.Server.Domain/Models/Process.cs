namespace Viewer.Server.Domain.Models;

public enum ProcessStatus
{
	Active,
	Blocked
}

public class Process : ITrackable
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public ProcessStatus Status { get; set; } = ProcessStatus.Active;
	
	public DateTimeOffset? CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
	
	public virtual ICollection<ProcessInConfiguration> Configurations { get; set; } = new List<ProcessInConfiguration>();
}