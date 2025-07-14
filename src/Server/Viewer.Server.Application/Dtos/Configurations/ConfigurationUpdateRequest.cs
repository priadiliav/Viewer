namespace Viewer.Server.Application.Dtos;

public class ConfigurationUpdateRequest
{
	public string Name { get; set; } = string.Empty;
	public List<long> PolicyIds { get; set; } = new();
	public List<long> ProcessIds { get; set; } = new();
}
