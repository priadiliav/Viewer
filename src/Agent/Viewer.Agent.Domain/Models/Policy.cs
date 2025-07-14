namespace Viewer.Agent.Domain.Models;

public enum RegistryKeyType
{
    Hklm, // HKEY_LOCAL_MACHINE
    Hkcu, // HKEY_CURRENT_USER
    Hkcr, // HKEY_CLASSES_ROOT
    Hkus, // HKEY_USERS
}

public enum RegistryValueKind
{
	String,
	Binary,
	DWord,
	QWord
}

public class Policy
{
	public string Name { get; set; } = string.Empty;
	public string RegistryPath { get; set; } = string.Empty;
	
	public RegistryKeyType RegistryKeyType { get; set; }
	public string RegistryKey { get; set; } = string.Empty;
	
	public RegistryValueKind RegistryValueKind { get; set; }
	public string RegistryValue { get; set; } = string.Empty;
}
