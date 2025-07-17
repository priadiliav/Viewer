using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Interfaces.Handlers;

public interface IHeartbeatHandler : IMessageHandler<Heartbeat>;
