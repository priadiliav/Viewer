using Communication.AgentToServer;
using Google.Protobuf.WellKnownTypes;
using Viewer.Agent.Application.Interfaces.Producers;

namespace Viewer.Agent.Infrastructure.Grpc;

public class GrpcMessageProducer(IStreamClientManager streamClientManager) : IMessageProducer
{
    public async Task ProduceAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class
    {
        if(message is null)
            throw new ArgumentNullException(nameof(message));
        
        var grpcObject = BuildGrpcMessage(message);
        
        await streamClientManager.SendMessageAsync(grpcObject, cancellationToken);
    }
    
    private AgentToServerMessage BuildGrpcMessage<TMessage>(TMessage message) where TMessage : class
    {
        return message switch
        {
            Domain.Models.Heartbeat heartbeat => new AgentToServerMessage
            {
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                Heartbeat = heartbeat.ToGrpc()
            },
    
            _ => throw new NotSupportedException($"GrpcMessageProducer does not support message type {typeof(TMessage).Name}")
        };
    }
}
