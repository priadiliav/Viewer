using System.Collections.Concurrent;
using Communication;
using Communication.AgentToServer;
using Communication.ServerToAgent;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Viewer.Server.Application.Interfaces.Services;
using Viewer.Server.Infrastructure.Grpc.Handlers;

namespace Viewer.Server.Infrastructure.Grpc;


public interface IGrpcStreamManager
{
	void AddStream(Guid agentId, IAsyncStreamReader<AgentToServerMessage> reader, IAsyncStreamWriter<ServerToAgentMessage> writer);
	void RemoveStream(Guid agentId);
	Task SendMessageAsync(Guid agentId, ServerToAgentMessage message);
	Task HandleIncomingMessagesAsync(Guid agentId);
}


public class GrpcStreamManager(
	ILogger<GrpcStreamManager> logger, 
	IServiceScopeFactory scopeFactory) : IGrpcStreamManager, IStreamManager
{
	private record GrpcAgentConnection(
		IAsyncStreamReader<AgentToServerMessage> Reader,
		IAsyncStreamWriter<ServerToAgentMessage> Writer
	);
	
	private readonly ConcurrentDictionary<Guid, GrpcAgentConnection> _streams = new();
	
	public List<Guid> GetConnectedAgentIds() 
		=> _streams.Keys.ToList();

	public bool IsAgentConnected(Guid agentId) 
		=> _streams.ContainsKey(agentId);

	public void AddStream(
		Guid agentId, 
		IAsyncStreamReader<AgentToServerMessage> reader, 
		IAsyncStreamWriter<ServerToAgentMessage> writer) 
		=> _streams[agentId] = new GrpcAgentConnection(reader, writer);

	public void RemoveStream(Guid agentId)
		=> _streams.TryRemove(agentId, out _);

	public async Task SendMessageAsync(Guid agentId, ServerToAgentMessage message)
	{
		if (!_streams.TryGetValue(agentId, out var stream))
			throw new Exception($"Agent {agentId} not connected");

		try
		{
			await stream.Writer.WriteAsync(message);
		}
		catch (Exception ex)
		{
			logger.LogError($"Error sending messages to agent {agentId}: {ex.Message}");
			RemoveStream(agentId);
			throw;
		}
	}

	public Task HandleIncomingMessagesAsync(Guid agentId)
	{
		if (!_streams.TryGetValue(agentId, out var stream))
			throw new Exception($"Agent {agentId} not connected");
		
		return Task.Run(async () =>
		{
			try
			{
				while (await stream.Reader.MoveNext())
				{
					using var scope = scopeFactory.CreateScope();
					var messageHandlerFactory = scope.ServiceProvider.GetRequiredService<IMessageHandlerFactory>();
					await messageHandlerFactory.HandleAsync(stream.Reader.Current, agentId);
				}
			}
			catch (Exception ex)
			{
				logger.LogError($"Error handling messages for agent {agentId}: {ex.Message}");
				RemoveStream(agentId);
				throw;
			}
		});
	}
}