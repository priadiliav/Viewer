using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Viewer.Server.Application.Services;
using Viewer.Server.Domain.Models;
using Viewer.Server.Infrastructure.Grpc.Mappers;
using Viewer.Server.Infrastructure.Grpc.Services;

namespace Viewer.Server.Infrastructure.Grpc.Producers;

public class GrpcConfigurationProducer(
		ILogger<GrpcConfigurationProducer> logger,
		IGrpcStreamManager streamManager) : IConfigurationProducer
{
	public async Task ProduceAsync(Guid agentId, Configuration message)
	{
		if (message is null)
			throw new ArgumentNullException(nameof(message));

		if (agentId == Guid.Empty)
			throw new ArgumentException("AgentId cannot be empty", nameof(agentId));

		var configurationObj = message.ToGrpc();
		
		var grpcMessage = new Communication.ServerToAgent.ServerToAgentMessage()
		{
			Timestamp = DateTime.UtcNow.ToTimestamp(),
			Configuration = configurationObj
		};
		
		await streamManager.SendMessageAsync(agentId, grpcMessage);
		logger.LogInformation("Configuration message sent to agent {AgentId} at {Timestamp}", 
			agentId, grpcMessage.Timestamp.ToDateTime());
	}
}
