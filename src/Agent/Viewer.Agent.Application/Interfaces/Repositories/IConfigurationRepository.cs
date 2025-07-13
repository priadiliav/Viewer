using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Interfaces.Repositories;

public interface IConfigurationRepository
{
	Configuration? GetConfiguration();
	void SaveConfiguration(Configuration configuration);
}