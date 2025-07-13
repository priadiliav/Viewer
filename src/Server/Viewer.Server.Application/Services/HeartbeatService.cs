using Viewer.Server.Application.Dtos;
using Viewer.Server.Application.Dtos.Heartbeat;
using Viewer.Server.Application.Interfaces.Repositories;

namespace Viewer.Server.Application.Services;

public interface IHeartbeatService
{
	Task<IEnumerable<HeartbeatDto>> GetAllAsync();
}

public class HeartbeatService(IHeartbeatRepository heartbeatRepository) : IHeartbeatService
{
	public async Task<IEnumerable<HeartbeatDto>> GetAllAsync()
	{
		var heartbeatsDomain = await heartbeatRepository.GetAllAsync();
		return heartbeatsDomain.Select(x => x.ToDto());
	}
}