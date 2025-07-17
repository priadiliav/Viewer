using Viewer.Agent.Application.Interfaces.Producers;
using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Producers;

public class HeartbeatProducer(IMessageProducer producer) : IHeartbeatProducer
{
    public async Task ProduceAsync(Heartbeat heartbeat, CancellationToken cancellationToken = default)
    {
       await producer.ProduceAsync(heartbeat, cancellationToken); 
    }
}
