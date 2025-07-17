namespace Viewer.Agent.Application.Interfaces.Producers;

public interface IMessageProducer
{
	Task ProduceAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class;
}
