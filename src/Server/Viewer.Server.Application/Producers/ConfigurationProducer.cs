using Viewer.Server.Application.Interfaces.Producers;
using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Producers;

public class ConfigurationProducer(IMessageProducer messageProducer) : IConfigurationProducer
{
    public async Task ProduceAsync(Guid agentId, Configuration message)
    {
        if (message is null) 
            throw new ArgumentNullException(nameof(message), "Configuration message cannot be null.");

        await messageProducer.ProduceAsync(agentId, message);
    }
}
