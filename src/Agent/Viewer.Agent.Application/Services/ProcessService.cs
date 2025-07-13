using Microsoft.Extensions.Logging;
using Viewer.Agent.Application.Dtos;
using Viewer.Agent.Application.Interfaces.Repositories;
using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Services;


public interface IProcessService
{
	IEnumerable<Process> GetAllConfigProcesses();
	IEnumerable<Process> GetConfigProcessesByStatus(ProcessStatus status);
	Task StartProcessWatcherAsync(CancellationToken cancellationToken = default);
	Task StopProcessWatcherAsync(CancellationToken cancellationToken = default);
}
public class ProcessService(
		ILogger<ProcessService> logger,
		IProcessRepository processRepository) : IProcessService
{
	public IEnumerable<Process> GetAllConfigProcesses()
	{
		return processRepository.GetConfigProcesses();
	}
	
	public IEnumerable<Process> GetConfigProcessesByStatus(ProcessStatus status)
	{
		return processRepository.GetConfigProcessesByStatus(status);
	}
	
	public async Task StartProcessWatcherAsync(CancellationToken cancellationToken = default)
	{
		await processRepository.StartProcessWatcherAsync(
			onProcessStarted: (name, pid) =>
			{
				var blockedProcesses = processRepository.GetConfigProcessesByStatus(ProcessStatus.Blocked);
				processRepository.KillProcessesByNames(blockedProcesses.Select(p => p.Name));
			},
			cancellationToken);
	}
	
	public async Task StopProcessWatcherAsync(CancellationToken cancellationToken = default)
	{
		await processRepository.StopProcessWatcherAsync((name, pid) =>
		{
			// todo: handle process stopped event if needed
		}, cancellationToken);
	}
}