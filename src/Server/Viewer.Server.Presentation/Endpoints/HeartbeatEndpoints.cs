using Viewer.Server.Application.Dtos.Heartbeat;
using Viewer.Server.Application.Services;

namespace Viewer.Server.Presentation.Endpoints;

public static class HeartbeatEndpoints 
{
	public static async Task<IEnumerable<HeartbeatDto>> GetHeartbeats(IHeartbeatService heartbeatService)
	{
		var heartbeats = await heartbeatService.GetAllAsync();
		return heartbeats;
	}

	public static void MapHeartbeatEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/api/heartbeats").WithTags("Heartbeats");

		group.MapGet("/", GetHeartbeats).WithName("GetHeartbeats")
			.Produces<IEnumerable<HeartbeatDto>>()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.WithSummary("Get all heartbeats");
	}
}