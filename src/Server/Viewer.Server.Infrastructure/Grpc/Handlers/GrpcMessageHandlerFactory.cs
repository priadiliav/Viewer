using Communication.AgentToServer;
using Microsoft.Extensions.DependencyInjection;
using Viewer.Server.Application.Interfaces.Handlers;
using Viewer.Server.Infrastructure.Grpc.Mappers;

namespace Viewer.Server.Infrastructure.Grpc.Handlers;

public interface IMessageHandlerFactory
{
	Task HandleAsync(AgentToServerMessage payloadOneofCase, Guid agentId);
}

public class GrpcMessageHandlerFactory(IServiceProvider serviceProvider) : IMessageHandlerFactory
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
