using Communication.ServerToAgent;
using Microsoft.Extensions.DependencyInjection;
using Viewer.Agent.Application.Interfaces.Handlers;

namespace Viewer.Agent.Infrastructure.Grpc;

public interface IHandlerResolver
{
	Task HandleAsync(ServerToAgentMessage payloadOneofCase);
}

public class GrpcHandlerResolver(IServiceProvider serviceProvider) : IHandlerResolver
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
