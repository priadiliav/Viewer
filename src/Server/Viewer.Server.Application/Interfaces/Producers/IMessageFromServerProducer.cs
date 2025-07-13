namespace Viewer.Server.Application.Interfaces.Producers;

public interface IMessageFromServerProducer<in TMessage> where TMessage : class
{
	Task ProduceAsync(Guid agentId, TMessage message);
}