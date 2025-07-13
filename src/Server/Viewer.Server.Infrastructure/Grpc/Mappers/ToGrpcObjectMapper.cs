using Viewer.Server.Application.Dtos;
using Viewer.Server.Domain.Models;

namespace Viewer.Server.Infrastructure.Grpc;

public static class ToGrpcObjectMapper
{
	public static Communication.Common.Configuration ToGrpc(this Configuration configuration)
	{
		return new Communication.Common.Configuration
		{
			Name = configuration.Name,
			Policies = { configuration.Policies.Select(p => new Communication.Common.Policy
			{
				Name = p.Policy.Name,
				RegistryPath = p.Policy.RegistryPath,
				RegistryValue = p.Policy.RegistryValue,
				RegistryValueType = (Communication.Common.RegistryValueType)p.Policy.RegistryValueType,
				RegistryKey = p.Policy.RegistryKey,
				RegistryKeyType = (Communication.Common.RegistryKeyType)p.Policy.RegistryKeyType,
			}) },
			Processes = { configuration.Processes.Select(p => new Communication.Common.Process
			{
				Name = p.Process.Name,
				Status = (Communication.Common.ProcessStatus)p.Process.Status,
			}) },
		};
	}
	
	
}