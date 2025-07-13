using Communication.AgentToServer;
using Communication.ServerToAgent;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Viewer.Server.Application.Services;

namespace Viewer.Server.Infrastructure.Grpc;

public class GrpcStreamService(
	ILogger<GrpcStreamService> logger, 
	IAgentService agentService,
	IGrpcStreamManager streamManager) : Communication.StreamService.StreamServiceBase
{
	public override async Task Communicate(
			IAsyncStreamReader<AgentToServerMessage> requestStream, 
			IServerStreamWriter<ServerToAgentMessage> responseStream, 
			ServerCallContext context)
	{
		var agentId = context.RequestHeaders.ExtractAgentId();
		if (agentId == Guid.Empty)
		{
			logger.LogError("Invalid agent ID in request headers");
			throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid agent ID"));
		}
		
		var token = context.RequestHeaders.ExtractToken();
		var isAuthenticated = await agentService.AuthenticateAsync(token);
		if(isAuthenticated is not true)
		{
			logger.LogError("Authentication failed for agent {AgentId}", agentId);
			throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid token"));
		}
		
		streamManager.AddStream(agentId, requestStream, responseStream);
		
		_ = streamManager.HandleIncomingMessagesAsync(agentId);
		try
		{
			logger.LogInformation("Stream for agent {AgentId} has been established", agentId);
			
			await Task.Delay(Timeout.Infinite, context.CancellationToken);
		}
		catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
		{
			logger.LogWarning("Stream for agent {AgentId} was cancelled", agentId);
		}
		catch (Exception ex)
		{
			logger.LogError("Error handling stream for agent {AgentId}", agentId);
		}
		finally
		{
			streamManager.RemoveStream(agentId);
			logger.LogInformation("Stream for agent {AgentId} has been removed", agentId);
		}
	}
}