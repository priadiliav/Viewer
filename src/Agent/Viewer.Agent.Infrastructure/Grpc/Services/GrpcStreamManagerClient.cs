using Communication.AgentToServer;
using Communication.ServerToAgent;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Viewer.Agent.Domain.Configs;
using Viewer.Agent.Infrastructure.Grpc.Handlers;
using Viewer.Agent.Infrastructure.Grpc.Utils;
using AuthContext = Viewer.Agent.Domain.Configs.AuthContext;

namespace Viewer.Agent.Infrastructure.Grpc.Services;

public interface IStreamManagerClient
{
	Task StartStreamAsync(CancellationToken cancellationToken = default);
	Task SendMessageAsync(AgentToServerMessage message, CancellationToken cancellationToken = default);
	Task HandleMessagesAsync(CancellationToken cancellationToken = default);
	ValueTask DisposeAsync();
}

public class GrpcStreamManagerClient(
	ILogger<GrpcStreamManagerClient> logger,
	IOptions<AgentConfig> agentConfig,
	AuthContext authContext,
	IMessageHandlerFactory messageHandlerFactory,
	Communication.StreamService.StreamServiceClient streamServiceClient) : IStreamManagerClient
{
	private AsyncDuplexStreamingCall<AgentToServerMessage, ServerToAgentMessage>? _call;
	private readonly SemaphoreSlim _streamLock = new(1, 1);
	private bool _isDisposed;


	/// <summary>
	/// Sets the gRPC call for duplex streaming.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task StartStreamAsync(CancellationToken cancellationToken = default)
	{
		if (_isDisposed) 
			throw new ObjectDisposedException(nameof(GrpcStreamManagerClient));
		
		var retryPolicy = Policy
				.Handle<RpcException>(IsTransientGrpcError) 
				.WaitAndRetryAsync(
						retryCount: 3,
						sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
						onRetry: (exception, timespan, retryCount, context) =>
						{
							logger.LogWarning(exception, "Retry {RetryCount} after {Delay} due to failure starting stream", retryCount, timespan);
						});

		await retryPolicy.ExecuteAsync(async ct =>
		{
			await _streamLock.WaitAsync(ct);
			
			try
			{
				if (_call != null)
				{
					logger.LogWarning("Stream is already initialized. Disposing the previous stream.");
					await DisposeStreamAsync();
				}

				var metadata = new Metadata()
				{
						{ "agent-id", agentConfig.Value.Id },
						{ "authorization", $"Bearer {authContext.Token}" }
				};

				_call = streamServiceClient.Communicate(metadata, cancellationToken: ct);
				logger.LogInformation("Stream call has been set successfully.");
			}
			finally
			{
				_streamLock.Release();
			}
		}, cancellationToken);
	}

	/// <summary>
	/// Sends a message to the server.
	/// </summary>
	/// <param name="message"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public async Task SendMessageAsync(AgentToServerMessage message, CancellationToken cancellationToken = default)
	{
		if (_isDisposed) 
			throw new ObjectDisposedException(nameof(GrpcStreamManagerClient));

		if (_call is null or { RequestStream: null })
			throw new InvalidOperationException("Stream is not started. Call StartStreamAsync first.");

		var retryPolicy = Policy
				.Handle<RpcException>(IsTransientGrpcError)
				.WaitAndRetryAsync(
						retryCount: 3,
						sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
						onRetry: (exception, timespan, retryCount, context) =>
						{
							logger.LogWarning(exception, "Retry {RetryCount} after {Delay} due to heartbeat send failure", retryCount, timespan);
						});
		
		await retryPolicy.ExecuteAsync(async ct =>
		{
			await _call.RequestStream.WriteAsync(message, cancellationToken: ct);
			logger.LogInformation("Message sent: {MessageType}", message.GetType().Name);
		}, cancellationToken);
	}
	
	/// <summary>
	/// Checks if the gRPC error is transient.
	/// </summary>
	/// <param name="ex"></param>
	/// <returns></returns>
	/// todo: separate exceptions statuses by needs to shut down or retry
	private bool IsTransientGrpcError(RpcException ex)
	{
		return ex.StatusCode switch
		{
				StatusCode.Unavailable => true,
				StatusCode.DeadlineExceeded => true,
				StatusCode.Internal => true,
				StatusCode.ResourceExhausted => true,
				StatusCode.Unauthenticated => false,
				StatusCode.PermissionDenied => false,
				_ => false
		};
	}
	/// <summary>
	/// Handles incoming messages from the server.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public async Task HandleMessagesAsync(CancellationToken cancellationToken = default)
	{
		if (_isDisposed) throw new ObjectDisposedException(nameof(GrpcStreamManagerClient));

		if (_call == null || _call.ResponseStream == null)
			throw new InvalidOperationException("Stream is not started. Call StartStreamAsync first.");

		try
		{
			while (await _call.ResponseStream.MoveNext(cancellationToken))
			{
				var message = _call.ResponseStream.Current;
				
				await messageHandlerFactory.HandleAsync(message);
			}
		}
		catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled && cancellationToken.IsCancellationRequested)
		{
			logger.LogInformation("Stream cancelled.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error receiving messages.");
			throw;
		}
		finally
		{
			await DisposeStreamAsync();
		}
	}
	
	
	/// <summary>
	/// Disposes the gRPC stream and completes the request stream.
	/// </summary>
	private async Task DisposeStreamAsync()
	{
		if (_call != null)
		{
			try
			{
				await _call.RequestStream.CompleteAsync();
			}
			catch
			{
				// Ignore any exceptions during completion	
			}

			_call.Dispose();
			_call = null;

			logger.LogInformation("Stream disposed.");
		}
	}
	
	/// <summary>
	/// Disposes the gRPC stream manager client.
	/// </summary>
	public async ValueTask DisposeAsync()
	{
		if (_isDisposed) return;
		_isDisposed = true;

		await _streamLock.WaitAsync();
		try
		{
			await DisposeStreamAsync();
		}
		finally
		{
			_streamLock.Release();
		}
	}
}