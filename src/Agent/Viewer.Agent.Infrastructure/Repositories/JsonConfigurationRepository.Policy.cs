namespace Viewer.Agent.Infrastructure.Repositories;

public partial class JsonConfigurationRepository
{
	public List<Domain.Models.Policy> GetPolicies()
	{
		var configuration = GetConfiguration();
		if (configuration == null)
		{
			throw new InvalidOperationException("Configuration is null. Cannot retrieve policies.");
		}
		
		return configuration.Policies;
	}
}