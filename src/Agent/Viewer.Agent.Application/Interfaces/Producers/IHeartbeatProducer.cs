using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Interfaces.Producers;

public interface IHeartbeatProducer : IMessageFromAgentProducer<Heartbeat>;