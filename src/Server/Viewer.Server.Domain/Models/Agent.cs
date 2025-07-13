using Viewer.Server.Domain.Interfaces;

namespace Viewer.Server.Domain.Models;

public class Agent : ITrackable
{
	public Guid Id { get; init; }
	public string Name { get; set; } = string.Empty;
	public long ConfigurationId { get; set; }
	
	public DateTimeOffset? CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
	
	public virtual Configuration Configuration { get; set; } = null!;
	
	public void UpdateFrom(Agent agent)
	{
		if (agent == null) 
			throw new ArgumentNullException(nameof(agent));
		
		Name = agent.Name;
		ConfigurationId = agent.ConfigurationId;
	}
}