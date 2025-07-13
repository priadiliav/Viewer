using Viewer.Agent.Application.Interfaces.Repositories;
using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Services;

public interface IConfigurationService
{
	Task ApplyConfigurationAsync(Configuration configuration);
}

public class ConfigurationService(IConfigurationRepository configurationRepository) : IConfigurationService
{
	public async Task ApplyConfigurationAsync(Configuration configuration)
	{
		await Task.Delay(2000);
		// apply configuration
		
		configurationRepository.SaveConfiguration(configuration);
	}
}