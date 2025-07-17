using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Interfaces.Producers;

public interface IHeartbeatProducer
{
    Task ProduceAsync(Heartbeat heartbeat, CancellationToken cancellationToken = default);
}
