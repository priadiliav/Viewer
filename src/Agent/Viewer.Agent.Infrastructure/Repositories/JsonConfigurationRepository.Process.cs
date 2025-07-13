using Viewer.Agent.Application.Interfaces;
using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Infrastructure.Repositories;

public partial class JsonConfigurationRepository
{
	public List<Domain.Models.Process> GetProcesses()
	{
		var configuration = GetConfiguration();
		if (configuration == null)
		{
			throw new InvalidOperationException("Configuration is null. Cannot retrieve processes.");
		}
		
		return configuration.Processes;
	}
	
	public List<Domain.Models.Process> GetProcessesByStatus(ProcessStatus status)
	{
		var processes = GetProcesses();
		return processes.Where(p => p.Status == status).ToList();
	}
}