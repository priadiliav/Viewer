namespace Viewer.Agent.Infrastructure.Grpc;

public static class ToGrpcObjectMapper
{
	public static Communication.AgentToServer.Heartbeat ToGrpc(this Domain.Models.Heartbeat heartbeat)
	{
		return new Communication.AgentToServer.Heartbeat();
	}
}
