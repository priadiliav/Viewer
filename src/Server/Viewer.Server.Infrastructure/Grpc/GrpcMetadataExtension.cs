using Grpc.Core;

namespace Viewer.Server.Infrastructure.Grpc;

public static class GrpcMetadataExtensions
{
	private const string AgentIdHeaderKey = "agent-id";
	private const string TokenHeaderKey = "authorization";
	
	public static Guid ExtractAgentId(this Metadata headers)
	{
		var entry = headers.FirstOrDefault(e =>
			string.Equals(e.Key, AgentIdHeaderKey, StringComparison.OrdinalIgnoreCase));

		if (entry == null || string.IsNullOrWhiteSpace(entry.Value))
			throw new RpcException(new Status(StatusCode.InvalidArgument, "Missing 'agent-id' metadata"));

		if (!Guid.TryParse(entry.Value, out var agentId))
			throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid 'agent-id' format"));

		return agentId;
	}

	public static string ExtractToken(this Metadata headers)
	{
		var entry = headers.FirstOrDefault(e =>
				string.Equals(e.Key, TokenHeaderKey, StringComparison.OrdinalIgnoreCase));

		if (entry == null || string.IsNullOrWhiteSpace(entry.Value))
			throw new RpcException(new Status(StatusCode.InvalidArgument, "Missing 'authorization' metadata"));

		if (!entry.Value.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
			throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid 'authorization' format"));

		return entry.Value.Substring("Bearer ".Length).Trim();
	}
}
