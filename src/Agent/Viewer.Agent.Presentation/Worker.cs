using Communication.AgentToServer;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Viewer.Agent.Application.Services;
using Viewer.Agent.Domain.Configs;
using Viewer.Agent.Infrastructure.Grpc;

namespace Viewer.Agent.Presentation;

public class Worker(
		IOptions<AgentConfig> agentConfigOptions,
		IManagementClientService managementClientService,
		IStreamClientManager streamClientManager,
		IHeartbeatService heartbeatService,
		IProcessService processService,
		ILogger<Worker> logger) : BackgroundService
{
	private readonly AgentConfig _agentConfig = agentConfigOptions.Value;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		try
		{
			await managementClientService.LoginAsync(stoppingToken);
			logger.LogInformation("Agent successfully logged in");
		}
		catch (Exception ex)
		{
			logger.LogCritical(ex, "Failed to authenticate agent. Shutting down...");
			return;
		}

		try
		{
			logger.LogInformation("Starting stream manager client...");
			await streamClientManager.StartStreamAsync(cancellationToken: stoppingToken);
			logger.LogInformation("Stream has been started successfully");
		}
		catch (Exception ex)
		{
			logger.LogCritical(ex, "Failed to start stream manager client. Shutting down...");
			return;
		}

		try
		{
			logger.LogInformation("Handling messages from stream manager client...");
			var handleMessagesAsync = streamClientManager.HandleMessagesAsync(stoppingToken);
			logger.LogInformation("Message handling started successfully");
			
			logger.LogInformation("Starting heartbeat producing...");
			var produceHeartbeatsAsync = heartbeatService.ProduceHeartbeatsAsync(stoppingToken);
			logger.LogInformation("Heartbeat producing started successfully");

			logger.LogInformation("Starting process watchers...");
			var processStartWatcherTask = processService.StartProcessWatcherAsync(stoppingToken);
			var processStopWatcherTask = processService.StopProcessWatcherAsync(stoppingToken);
			logger.LogInformation("Process watchers started successfully");
			
			await Task.WhenAll(
					// Periodic producers
					produceHeartbeatsAsync,
					// Handlers
					handleMessagesAsync,
					// Watchers
					processStartWatcherTask,
					processStopWatcherTask);
		}
		catch (OperationCanceledException)
		{
			logger.LogInformation("Worker is stopping due to cancellation.");
		}
		finally
		{
			logger.LogInformation("Worker is cleaning up.");

			try
			{
				await streamClientManager.DisposeAsync();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error during stream manager cleanup.");
			}
		}
	}
}

