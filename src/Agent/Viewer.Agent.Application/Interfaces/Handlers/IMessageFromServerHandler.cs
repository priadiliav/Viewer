namespace Viewer.Agent.Application.Interfaces.Handlers;

public interface IMessageFromServerHandler<in TMessage> where TMessage : class
{
	Task HandleAsync(TMessage message);
}