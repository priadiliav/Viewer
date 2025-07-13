namespace Viewer.Agent.Domain.Models;

public class Configuration
{
    public string Name { get; set; } = string.Empty;
    public List<Process> Processes { get; set; } = new();
    public List<Policy> Policies { get; set; } = new();
}
