namespace Viewer.Server.Application.Dtos.Agent;

public class AgentUpdateRequest
{
    public string Name { get; set; } = string.Empty;
    public long ConfigurationId { get; set; }
}
