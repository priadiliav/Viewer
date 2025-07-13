using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Viewer.Agent.Application.Interfaces.Producers;
using Viewer.Agent.Domain.Models;
using Viewer.Agent.Infrastructure.Grpc.Mappers;
using Viewer.Agent.Infrastructure.Grpc.Services;

namespace Viewer.Agent.Infrastructure.Grpc.Producers;

public class GrpcHearbeatProducer(
		ILogger<GrpcHearbeatProducer> logger,
		IStreamManagerClient streamManagerClient) : IHeartbeatProducer
{
	public async Task ProduceAsync(Heartbeat message, CancellationToken cancellationToken = default)
	{
		if(message is null)
			throw new ArgumentNullException(nameof(message));

		var heartbeatObj = message.ToGrpc();
		
		var grpcMessage = new Communication.AgentToServer.AgentToServerMessage()
		{
			Timestamp = DateTime.UtcNow.ToTimestamp(),
			Heartbeat = heartbeatObj
		};

		await streamManagerClient.SendMessageAsync(grpcMessage, cancellationToken);
		logger.LogInformation("Heartbeat message sent at {Timestamp}", grpcMessage.Timestamp.ToDateTime());
	}
}