using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Interfaces.Producers;

public interface IConfigurationProducer
{
    Task ProduceAsync(Guid agentId, Configuration message);
}
