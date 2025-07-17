namespace Viewer.Agent.Domain.Configs;

public class RepositoryConfig
{
	public string Path { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
	public string Extension { get; init; } = string.Empty;
	public string GetFullPath() => System.IO.Path.Combine(Path, Name + Extension);
}
