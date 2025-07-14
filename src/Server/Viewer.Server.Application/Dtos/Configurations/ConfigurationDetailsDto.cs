using Viewer.Server.Application.Dtos.Agent;
using Viewer.Server.Application.Dtos.Policy;
using Viewer.Server.Application.Dtos.Process;

namespace Viewer.Server.Application.Dtos;

public class ConfigurationDetailsDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
    public bool IsApplied { get; set; } = false;
	public List<AgentDto> Agents { get; set; } = new();
	public List<PolicyDto> Policies { get; set; } = new();
	public List<ProcessDto> Processes { get; set; } = new();
}
