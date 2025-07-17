using Communication.ServerToAgent;
using Google.Protobuf.WellKnownTypes;
using Viewer.Server.Application.Interfaces.Producers;
using Viewer.Server.Domain.Models;

namespace Viewer.Server.Infrastructure.Grpc;

public class GrpcMessageProducer(IGrpcStreamManager streamManager) : IMessageProducer
{
    public async Task ProduceAsync<TMessage>(Guid agentId, TMessage message) where TMessage : class
    {
        if (message is null)
            throw new ArgumentNullException(nameof(message));
         
        if (agentId == Guid.Empty)
            throw new ArgumentException("AgentId cannot be empty", nameof(agentId));

        var grpcMessage = BuildGrpcMessage(agentId, message);
         
        await streamManager.SendMessageAsync(agentId, grpcMessage);
    }
    
    private ServerToAgentMessage BuildGrpcMessage<TMessage>(Guid agentId, TMessage message) where TMessage : class
    {
        return message switch
        {
            Configuration config => new ServerToAgentMessage
            {
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                Configuration = config.ToGrpc()
            },
        
            _ => throw new NotSupportedException($"GrpcMessageProducer does not support message type {typeof(TMessage).Name}")
        };
    }
}

