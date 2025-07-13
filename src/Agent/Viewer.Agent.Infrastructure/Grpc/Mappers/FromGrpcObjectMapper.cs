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
			RegistryKeyType = (RegistryKeyType)source.RegistryKeyType,
			RegistryKey = source.RegistryKey,
			RegistryValueKind = (RegistryValueKind)source.RegistryValueType,
			RegistryValue= source.RegistryValue
		};
	}
}