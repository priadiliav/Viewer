using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Infrastructure.Grpc.Mappers;

public static class FromGrpcObjectMapper
{
	public static Configuration ToDomain(this Communication.Common.Configuration grpcConfiguration)
	{
		return new Configuration
		{
			Name = grpcConfiguration.Name,
			Processes = grpcConfiguration.Processes.Select(ToDomain).ToList(),
			Policies = grpcConfiguration.Policies.Select(ToDomain).ToList(),
		};
	}

	public static Domain.Models.Process ToDomain(this Communication.Common.Process source)
	{
		return new Domain.Models.Process
		{
			Name = source.Name,
			Status = (ProcessStatus)source.Status
		};
	}
	
	public static Policy ToDomain(this Communication.Common.Policy source)
	{
		return new Policy
		{
			Name = source.Name,
			RegistryPath = source.RegistryPath,
			RegistryKeyType = ToDomainRegistryKey(source.RegistryKeyType),
			RegistryKey = source.RegistryKey,
			RegistryValueKind = (Microsoft.Win32.RegistryValueKind)source.RegistryValueType,
			RegistryValue= source.RegistryValue
		};
	}
	
	public static Microsoft.Win32.RegistryKey ToDomainRegistryKey(this Communication.Common.RegistryKeyType grpcRegistryKey)
	{
		return grpcRegistryKey switch
		{
			Communication.Common.RegistryKeyType.Hkcr => Microsoft.Win32.Registry.ClassesRoot,
			Communication.Common.RegistryKeyType.Hkcu => Microsoft.Win32.Registry.CurrentUser,
			Communication.Common.RegistryKeyType.Hklm => Microsoft.Win32.Registry.LocalMachine,
			Communication.Common.RegistryKeyType.Hkus => Microsoft.Win32.Registry.Users,
			_ => throw new ArgumentOutOfRangeException(nameof(grpcRegistryKey), grpcRegistryKey, null)
		};
	}
}