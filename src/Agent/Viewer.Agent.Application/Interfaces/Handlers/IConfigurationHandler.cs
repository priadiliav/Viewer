using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Interfaces.Handlers;

public interface IConfigurationHandler : IMessageFromServerHandler<Configuration>;