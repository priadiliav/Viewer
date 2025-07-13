using Communication.AgentToServer;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Viewer.Agent.Application.Services;
using Viewer.Agent.Domain.Configs;
using Viewer.Agent.Infrastructure.Grpc.Services;

namespace Viewer.Agent.Presentation;

public class Worker(
		IOptions<AgentConfig> agentConfigOptions,
		IManagementServiceClient managementServiceClient,
		IStreamManagerClient streamManagerClient,
		IHeartbeatService heartbeatService,
		ILogger<Worker> logger) : BackgroundService
{
	private readonly AgentConfig _agentConfig = agentConfigOptions.Value;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		try
		{
			await managementServiceClient.LoginAsync(stoppingToken);
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
			await streamManagerClient.StartStreamAsync(cancellationToken: stoppingToken);
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
			var handleMessagesAsync = streamManagerClient.HandleMessagesAsync(stoppingToken);
			logger.LogInformation("Message handling started successfully");
			
			logger.LogInformation("Starting heartbeat producing...");
			var produceHeartbeatsAsync = heartbeatService.ProduceHeartbeatsAsync(stoppingToken);
			logger.LogInformation("Heartbeat producing started successfully");

			await Task.WhenAll(
					produceHeartbeatsAsync,
					handleMessagesAsync);
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
				await streamManagerClient.DisposeAsync();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error during stream manager cleanup.");
			}
		}
	}
}

