namespace Viewer.Agent.Domain.Models;

public enum ProcessStatus
{
	Active,
	Blocked
}

public class Process
{
	public string Name { get; set; } = string.Empty;
	public ProcessStatus Status { get; set; } = ProcessStatus.Active;
}