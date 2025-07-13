using Microsoft.Win32;

namespace Viewer.Agent.Domain.Models;

public class Policy
{
	public string Name { get; set; } = string.Empty;
	public string RegistryPath { get; set; } = string.Empty;
	
	public RegistryKey RegistryKeyType { get; set; }
	public string RegistryKey { get; set; }
	
	public RegistryValueKind RegistryValueKind { get; set; }
	public object RegistryValue { get; set; }
}