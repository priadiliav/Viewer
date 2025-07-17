using Microsoft.Extensions.Logging;
using Viewer.Agent.Application.Interfaces.Producers;
using Viewer.Agent.Domain.Models;
using Polly;
using Polly.Retry;
using Policy = Polly.Policy;

namespace Viewer.Agent.Application.Services;

public interface IHeartbeatService
{
	Task SendHeartbeatAsync(Heartbeat heartbeat, CancellationToken cancellationToken = default);
	Task ProduceHeartbeatsAsync(CancellationToken cancellationToken = default);
}

public class HeartbeatService(ILogger<HeartbeatService> logger, IHeartbeatProducer heartbeatProducer) : IHeartbeatService
{
	public async Task SendHeartbeatAsync(Heartbeat heartbeat, CancellationToken cancellationToken = default)
	{
		if (heartbeat is null)
			throw new ArgumentNullException(nameof(heartbeat));

		var retryPolicy = Policy
			.Handle<Exception>()
			.WaitAndRetryAsync(
				retryCount: 3,
				sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
				onRetry: (exception, timespan, retryCount, context) =>
				{
					logger.LogWarning(exception, "Retry {RetryCount} after {Delay} due to heartbeat send failure", retryCount, timespan);
				});

		await retryPolicy.ExecuteAsync(async ct =>
		{
			await heartbeatProducer.ProduceAsync(heartbeat, ct);
			logger.LogInformation("Heartbeat sent at {Timestamp}", DateTime.UtcNow);
		}, cancellationToken);
	}


	public async Task ProduceHeartbeatsAsync(CancellationToken cancellationToken = default)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				var heartbeat = new Heartbeat
				{
				};

				await SendHeartbeatAsync(heartbeat, cancellationToken);
				
				await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error sending heartbeat");
				await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
			}
		}
	}
}
