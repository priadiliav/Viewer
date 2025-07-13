namespace Viewer.Agent.Application.Interfaces.Producers;

public interface IMessageFromAgentProducer<in TMessage> where TMessage : class
{
	Task ProduceAsync(TMessage message,
		CancellationToken cancellationToken = default);
}