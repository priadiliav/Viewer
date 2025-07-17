using Communication.AgentToServer;
using Microsoft.Extensions.DependencyInjection;
using Viewer.Server.Application.Interfaces.Handlers;

namespace Viewer.Server.Infrastructure.Grpc;

public interface IHandlerResolver
{
	Task HandleAsync(AgentToServerMessage payloadOneofCase, Guid agentId);
}

public class GrpcHandlerResolver(IServiceProvider serviceProvider) : IHandlerResolver
{
	public Task HandleAsync(AgentToServerMessage message, Guid agentId)
	{
		return message.PayloadCase switch
		{
			AgentToServerMessage.PayloadOneofCase.Heartbeat =>
					serviceProvider.GetRequiredService<IHeartbeatHandler>()
							.HandleAsync(message.ToHeartbeat(agentId)),
			
			_ => Task.CompletedTask
		};
	}
}
