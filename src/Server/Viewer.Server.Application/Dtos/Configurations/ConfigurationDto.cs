namespace Viewer.Server.Application.Dtos;

public class ConfigurationDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public List<Guid> AgentIds { get; set; } = new();
	public List<long> PolicyIds { get; set; } = new();
	public List<long> ProcessIds { get; set; } = new();
}