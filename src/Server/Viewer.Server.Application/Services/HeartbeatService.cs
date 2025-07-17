using Viewer.Server.Application.Dtos;
using Viewer.Server.Application.Dtos.Heartbeat;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Application.Interfaces.Services;

namespace Viewer.Server.Application.Services;

public class HeartbeatService(IHeartbeatRepository heartbeatRepository) : IHeartbeatService
{
	public async Task<IEnumerable<HeartbeatDto>> GetAllAsync()
	{
		var heartbeatsDomain = await heartbeatRepository.GetAllAsync();
		return heartbeatsDomain.Select(x => x.ToDto());
	}
}
