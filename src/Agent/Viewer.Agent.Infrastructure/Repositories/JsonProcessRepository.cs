using System.Management;
using Microsoft.Extensions.Logging;
using Viewer.Agent.Application.Interfaces.Repositories;
using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Infrastructure.Repositories;

public class JsonProcessRepository(
		ILogger<JsonProcessRepository> logger,
		IConfigurationRepository configurationRepository) : IProcessRepository
{
	private ManagementEventWatcher? _startWatcher;
	private ManagementEventWatcher? _stopWatcher;
	
	public IEnumerable<Domain.Models.Process> GetConfigProcesses()
	{
		var configuration = configurationRepository.GetConfiguration();
		return configuration?.Processes ?? [];
	}

	public IEnumerable<Domain.Models.Process> GetConfigProcessesByStatus(ProcessStatus status)
	{
		var processes = GetConfigProcesses();
		return processes.Where(p => p.Status == status).ToList();
	}

	public void SaveConfigProcesses(List<Domain.Models.Process> processes)
	{
		var configuration = configurationRepository.GetConfiguration() ?? new Configuration();
		configuration.Processes = processes;
		configurationRepository.SaveConfiguration(configuration);
	}
	
	private IEnumerable<System.Diagnostics.Process> GetAllRunningProcesses()
	{
		var processes = System.Diagnostics.Process.GetProcesses();
		return processes;
	}
	
	#region Kill Process Methods
	/// <summary>
	/// Kills a process by its name.
	/// </summary>
	/// <param name="processName"></param>
	public void KillProcessByName(string processName)
	{
		try
		{
			var processes = System.Diagnostics.Process.GetProcessesByName(processName);
			foreach (var process in processes)
			{
				process.Kill();
				process.WaitForExit();
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to kill process with name {ProcessName}", processName);
		}
	}

	/// <summary>
	/// Kills a process by its ID.
	/// </summary>
	/// <param name="processId"></param>
	public void KillProcessById(int processId)
	{
		try
		{
			var process = System.Diagnostics.Process.GetProcessById(processId);
			process.Kill();
			process.WaitForExit();
		}
		catch (ArgumentException ex)
		{
			logger.LogError("Process with ID {ProcessId} does not exist.", processId);
		}
	}

	/// <summary>
	/// Kills multiple processes by their IDs.
	/// </summary>
	/// <param name="processIds"></param>
	public void KillProcessesByIds(List<int> processIds)
	{
		foreach (var processId in processIds)
		{
			try
			{
				KillProcessById(processId);
			}
			catch (Exception ex)
			{
				logger.LogError("Failed to kill process with ID {ProcessId}", processId);
			}
		}
	}

	/// <summary>
	/// Kills multiple processes by their names.
	/// </summary>
	/// <param name="processNames"></param>
	public void KillProcessesByNames(IEnumerable<string> processNames)
	{
		foreach (var processName in processNames)
		{
			try
			{
				var processes = System.Diagnostics.Process.GetProcessesByName(processName);
				foreach (var process in processes)
				{
					process.Kill();
					process.WaitForExit();
				}
			}
			catch (Exception ex)
			{
				logger.LogError("Failed to kill process with name {ProcessName}", processName);
			}
		}
	}
	#endregion
	
	#region Process Watcher Methods
	/// <summary>
	/// Starts a watcher that listens for new processes being created.
	/// </summary>
	/// <param name="onProcessStarted"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task StartProcessWatcherAsync(Action<string, int> onProcessStarted, CancellationToken cancellationToken = default)
	{
		var query = new WqlEventQuery(
				"__InstanceCreationEvent",
				TimeSpan.FromSeconds(1),
				"TargetInstance isa 'Win32_Process'"
		);
		_startWatcher = new ManagementEventWatcher(query);
		_startWatcher.EventArrived += (sender, args) =>
		{
			try
			{
				var process = (ManagementBaseObject)args.NewEvent["TargetInstance"];
				var processName = process["Name"]?.ToString() ?? "Unknown";
				var processId = Convert.ToInt32(process["ProcessId"]);
			
				onProcessStarted(processName, processId);	
			}catch (Exception ex)
			{
				logger.LogError(ex, "Error processing new process event.");
			}
		};
		
		_startWatcher.Start();
		
		cancellationToken.Register(() => 
	          DisposeWatcher(_startWatcher, nameof(_startWatcher)));
		return Task.CompletedTask;
	}

	/// <summary>
	/// Start the watcher that listens for processes being stopped.
	/// </summary>
	/// <param name="onProcessStopped"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task StopProcessWatcherAsync(Action<string, int> onProcessStopped, CancellationToken cancellationToken = default)
	{
		var query = new WqlEventQuery(
				"__InstanceDeletionEvent",
				TimeSpan.FromSeconds(1),
				"TargetInstance isa 'Win32_Process'"
		);
		_stopWatcher = new ManagementEventWatcher(query);
		_stopWatcher.EventArrived += (sender, args) =>
		{
			try
			{
				var process = (ManagementBaseObject)args.NewEvent["TargetInstance"];
				var processName = process["Name"]?.ToString() ?? "Unknown";
				var processId = Convert.ToInt32(process["ProcessId"]);
			
				onProcessStopped(processName, processId);	
			}catch (Exception ex)
			{
				logger.LogError(ex, "Error processing stopped process event.");
			}
		};
		_stopWatcher.Start();
		
		cancellationToken.Register(() => 
				DisposeWatcher(_stopWatcher, nameof(_stopWatcher)));
		return Task.CompletedTask;
	}

	/// <summary>
	/// Stops the watcher that listens for new processes being created.
	/// </summary>
	/// <param name="watcher"></param>
	/// <param name="watcherName"></param>
	private void DisposeWatcher(ManagementEventWatcher? watcher, string watcherName)
	{
		try
		{
			watcher?.Stop();
			watcher?.Dispose();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to stop {WatcherName}", watcherName);
		}
	}
	#endregion
}