using Viewer.Server.Domain.Models;

namespace Viewer.Server.Application.Dtos.Policy;

public class PolicyDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
    
	public string RegistryPath { get; set; } = string.Empty;
	public string RegistryKey { get; set; } = string.Empty;

	public RegistryKeyType RegistryKeyType { get; set; }
	public RegistryType RegistryValueType { get; set; }
	
	public string RegistryValue { get; set; } = string.Empty;
}