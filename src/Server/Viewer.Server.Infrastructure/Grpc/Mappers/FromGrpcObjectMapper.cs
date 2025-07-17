using Communication.AgentToServer;

namespace Viewer.Server.Infrastructure.Grpc.Mappers;

public static class FromGrpcObjectMapper
{
	public static Domain.Models.Heartbeat ToHeartbeat(this AgentToServerMessage message, Guid agentId)
	{
		return new Domain.Models.Heartbeat
		{ 
			AgentId = agentId,
		};
	}
}
