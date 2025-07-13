using Viewer.Server.Application.Interfaces.Handlers;
using Viewer.Server.Application.Interfaces.Repositories;
using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Handlers;

public class HeartbeatHandler(IUnitOfWork unitOfWork) : IHeartbeatHandler
{
	public async Task HandleAsync(Heartbeat message)
	{
		if (message == null)
			throw new ArgumentNullException(nameof(message));

		if (message.AgentId == Guid.Empty)
			throw new ArgumentException("AgentId cannot be empty", nameof(message.AgentId));
		
		await unitOfWork.Heartbeats.CreateAsync(message);
		await unitOfWork.SaveChangesAsync();
	}
}