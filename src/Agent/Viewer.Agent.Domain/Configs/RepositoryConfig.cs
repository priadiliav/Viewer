namespace Viewer.Agent.Domain.Configs;

public class RepositoryConfig
{
	public string Path { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Extension { get; set; } = string.Empty;
	
	public string GetFullPath() => System.IO.Path.Combine(Path, Name + Extension);
}