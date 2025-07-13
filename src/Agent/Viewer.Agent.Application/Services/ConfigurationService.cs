using Viewer.Agent.Application.Interfaces.Repositories;
using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Services;

public interface IConfigurationService
{
	Task ApplyConfigurationAsync(Configuration configuration);
}

public class ConfigurationService(
		IProcessRepository processRepository,
		IPolicyRepository policyRepository,
		IConfigurationRepository configurationRepository) : IConfigurationService
{
	public async Task ApplyConfigurationAsync(Configuration configuration)
	{
		if (configuration == null)
			throw new ArgumentNullException(nameof(configuration));
		
		var applyProcessesTask = ApplyProcessesAsync(configuration.Processes);
		var applyPoliciesTask = ApplyPoliciesAsync(configuration.Policies);
		
		configurationRepository.SaveConfiguration(configuration);

		await Task.WhenAll(applyProcessesTask, applyPoliciesTask);
	}
	
	private Task ApplyProcessesAsync(IEnumerable<Domain.Models.Process> processes)
	{
		var blockedProcessesNames = processes
			.Where(p => p.Status == Domain.Models.ProcessStatus.Blocked)
			.Select(p => p.Name)
			.ToList();
		
		processRepository.KillProcessesByNames(blockedProcessesNames);
		
		return Task.CompletedTask;
	}

	private Task ApplyPoliciesAsync(IEnumerable<Domain.Models.Policy> policies)
	{
		policyRepository.SetPolicies(policies.ToList());
		
		return Task.CompletedTask;
	}
}