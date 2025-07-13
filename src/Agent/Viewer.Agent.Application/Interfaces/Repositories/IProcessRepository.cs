using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Interfaces.Repositories;

public interface IProcessRepository
{
	IEnumerable<Process> GetConfigProcesses();
	IEnumerable<Process> GetConfigProcessesByStatus(ProcessStatus status);
	void SaveConfigProcesses(List<Process> processes);
	void KillProcessByName(string processName);
	void KillProcessesByNames(IEnumerable<string> processNames);
	
	Task StartProcessWatcherAsync(Action<string, int> onProcessStarted, CancellationToken cancellationToken = default);
	Task StopProcessWatcherAsync(Action<string, int> onProcessStopped, CancellationToken cancellationToken = default);
}