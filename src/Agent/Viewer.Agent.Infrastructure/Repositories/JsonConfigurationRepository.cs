using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Viewer.Agent.Application.Interfaces.Repositories;
using Viewer.Agent.Domain.Configs;
using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Infrastructure.Repositories;

public class JsonConfigurationRepository(
		ILogger<JsonConfigurationRepository> logger, 
		IOptions<RepositoryConfig> repoConfig) : IConfigurationRepository
{
	private readonly string _filePath = repoConfig.Value.GetFullPath();
	private readonly object _lock = new();
	
	public Configuration? GetConfiguration()
	{
		lock (_lock)
		{
			try
			{
				if (!File.Exists(_filePath))
				{
					logger.LogWarning("Configuration file not found at {Path}", _filePath);
					return null;
				}

				var json = File.ReadAllText(_filePath);

				return JsonSerializer.Deserialize<Configuration>(json);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error reading configuration file");
				throw;
			}
		}
	}

	public void SaveConfiguration(Configuration configuration)
	{
		lock (_lock)
		{
			try
			{
				var json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions
				{
						WriteIndented = true
				});

				Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
				File.WriteAllText(_filePath, json);
				
				logger.LogInformation("Configuration successfully saved to {Path}", _filePath);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error saving configuration file");
			}
		}
	}
}