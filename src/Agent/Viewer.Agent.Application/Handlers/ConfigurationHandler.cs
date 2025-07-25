using Microsoft.Extensions.Logging;
using Viewer.Agent.Application.Interfaces.Handlers;
using Viewer.Agent.Application.Services;
using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Handlers;

public class ConfigurationHandler(ILogger<ConfigurationHandler> logger, IConfigurationService configurationService) : IConfigurationHandler
{
	public async Task HandleAsync(Configuration message)
	{
		logger.LogInformation("Received configuration with name {Name} and version", message.Name);
		await configurationService.ApplyConfigurationAsync(message);
	}
}