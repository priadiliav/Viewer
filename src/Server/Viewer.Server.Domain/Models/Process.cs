using Viewer.Server.Domain.Interfaces;

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
    
    public void UpdateFrom(Process process)
    {
        if (process == null) 
            throw new ArgumentNullException(nameof(process));

        Name = process.Name;
        Status = process.Status;

        // Clear existing configurations and add new ones
        Configurations.Clear();
        foreach (var config in process.Configurations)
        {
            Configurations.Add(config);
        }
    }
}
