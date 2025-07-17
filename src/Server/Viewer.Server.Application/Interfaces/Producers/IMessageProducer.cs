namespace Viewer.Server.Application.Interfaces.Producers;

public interface IMessageProducer
{
    Task ProduceAsync<TMessage>(Guid agentId, TMessage message) where TMessage : class;
}
