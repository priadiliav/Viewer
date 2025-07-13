using Viewer.Agent.Domain.Models;
using Process = System.Diagnostics.Process;

namespace Viewer.Agent.Application.Dtos;

public static class FromDomainMapper
{
	public static Process ToObj(this Domain.Models.Process process)
	{
		return new Process
		{
			
		};
	}
	
	public static Microsoft.Win32.RegistryKey ToDomainRegistryKey(this RegistryKeyType grpcRegistryKey)
	{
		return grpcRegistryKey switch
		{
			RegistryKeyType.Hkcr => Microsoft.Win32.Registry.ClassesRoot,
			RegistryKeyType.Hkcu => Microsoft.Win32.Registry.CurrentUser,
			RegistryKeyType.Hklm => Microsoft.Win32.Registry.LocalMachine,
			RegistryKeyType.Hkus => Microsoft.Win32.Registry.Users,
			_ => throw new ArgumentOutOfRangeException(nameof(grpcRegistryKey), grpcRegistryKey, null)
		};
	}
	
	public static Microsoft.Win32.RegistryValueKind ToDomainRegistryValueKind(this RegistryValueKind grpcRegistryValueKind)
	{
		return grpcRegistryValueKind switch
		{
			RegistryValueKind.String => Microsoft.Win32.RegistryValueKind.String,
			RegistryValueKind.Binary => Microsoft.Win32.RegistryValueKind.Binary,
			RegistryValueKind.DWord => Microsoft.Win32.RegistryValueKind.DWord,
			RegistryValueKind.QWord => Microsoft.Win32.RegistryValueKind.QWord,
			_ => throw new ArgumentOutOfRangeException(nameof(grpcRegistryValueKind), grpcRegistryValueKind, null)
		};
	}
}