using Communication.ServerToAgent;
using Microsoft.Extensions.DependencyInjection;
using Viewer.Agent.Application.Interfaces.Handlers;
using Viewer.Agent.Infrastructure.Grpc.Mappers;

namespace Viewer.Agent.Infrastructure.Grpc.Handlers;

public interface IMessageHandlerFactory
{
	Task HandleAsync(ServerToAgentMessage payloadOneofCase);
}

public class GrpcMessageHandlerFactory(IServiceProvider serviceProvider) : IMessageHandlerFactory
{
	public Task HandleAsync(ServerToAgentMessage message)
	{
		return message.PayloadCase switch
		{
			ServerToAgentMessage.PayloadOneofCase.Configuration =>
				serviceProvider.GetRequiredService<IConfigurationHandler>()
						.HandleAsync(message.Configuration.ToDomain()),
		
			_ => Task.CompletedTask
		};
	}
}