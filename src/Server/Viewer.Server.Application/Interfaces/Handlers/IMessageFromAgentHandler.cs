namespace Viewer.Server.Application.Interfaces.Handlers;

public interface IMessageFromAgentHandler<in TMessage> where TMessage : class
{
	Task HandleAsync(TMessage message);
}
