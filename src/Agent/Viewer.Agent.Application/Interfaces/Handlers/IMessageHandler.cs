namespace Viewer.Agent.Application.Interfaces.Handlers;

public interface IMessageHandler<in TMessage> where TMessage : class
{
	Task HandleAsync(TMessage message);
}
