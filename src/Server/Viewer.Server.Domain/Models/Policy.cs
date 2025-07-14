using Viewer.Server.Domain.Interfaces;

namespace Viewer.Server.Domain.Models;


public enum RegistryKeyType
{
	Hklm, // HKEY_LOCAL_MACHINE
	Hkcu, // HKEY_CURRENT_USER
	Hkcr, // HKEY_CLASSES_ROOT
	Hkus, // HKEY_USERS
}

public enum RegistryType
{
	String, // REG_SZ
	Binary, // REG_BINARY
	Dword, // REG_DWORD
	Qword, // REG_QWORD
}

public class Policy : ITrackable
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	
	public string RegistryPath { get; set; } = string.Empty;
	
	public RegistryKeyType RegistryKeyType { get; set; } = RegistryKeyType.Hkcu;
	public string RegistryKey { get; set; } = string.Empty;

	public RegistryType RegistryValueType { get; set; } = RegistryType.String;
	public string RegistryValue { get; set; } = string.Empty;
	
	public DateTimeOffset? CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
	
	public virtual ICollection<PolicyInConfiguration> Configurations { get; set; } = new List<PolicyInConfiguration>();

    public void UpdateFrom(Policy policy)
    {
        if (policy is null) 
            throw new ArgumentNullException(nameof(policy));

        Name = policy.Name;
        Description = policy.Description;
        RegistryPath = policy.RegistryPath;
        RegistryKeyType = policy.RegistryKeyType;
        RegistryKey = policy.RegistryKey;
        RegistryValueType = policy.RegistryValueType;
        RegistryValue = policy.RegistryValue;
    }
}
