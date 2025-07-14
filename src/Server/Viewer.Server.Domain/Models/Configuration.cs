using Viewer.Server.Domain.Interfaces;

namespace Viewer.Server.Domain.Models;

public class Configuration : ITrackable
{
	public long Id { get; init; }
	public string Name { get; set; } = string.Empty;
    public bool IsApplied { get; set; } = false;
	
	public DateTimeOffset? CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
	
	public virtual ICollection<Agent> Agents { get; set; } = new List<Agent>();
	public virtual ICollection<PolicyInConfiguration> Policies { get; set; } = new List<PolicyInConfiguration>();
	public virtual ICollection<ProcessInConfiguration> Processes { get; set; } = new List<ProcessInConfiguration>();
	
	public void UpdateFrom(Configuration configuration)
	{
		if (configuration == null) 
			throw new ArgumentNullException(nameof(configuration));

		Name = configuration.Name;

		Policies.Clear();
		foreach (var policy in configuration.Policies)
		{
			Policies.Add(policy);
		}

		Processes.Clear();
		foreach (var process in configuration.Processes)
		{
			Processes.Add(process);
		}
	}

    public void ResetAppliedStatus()
    {
        IsApplied = false;
    }
    
    public void MarkAsApplied()
    {
        IsApplied = true;
    }
}
