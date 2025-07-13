using Microsoft.Win32;
using Viewer.Agent.Application.Interfaces.Repositories;
using Viewer.Agent.Domain.Models;
using RegistryValueKind = Microsoft.Win32.RegistryValueKind;

namespace Viewer.Agent.Infrastructure.Repositories;

public class JsonPolicyRepository(IConfigurationRepository configurationRepository) : IPolicyRepository
{
	public List<Policy> GetConfigPolicies()
	{
		var configuration = configurationRepository.GetConfiguration();
		return configuration?.Policies ?? [];
	}

	public void SaveConfigPolicies(List<Policy> policies)
	{
		var configuration = configurationRepository.GetConfiguration() ?? new Configuration();
		configuration.Policies = policies;
		configurationRepository.SaveConfiguration(configuration);
	}
	
	
	public void SetPolicies(List<Policy>? policies)
	{
		if (policies is null || !policies.Any())
			return;

		foreach (var policy in policies)
		{
			SetPolicy(policy);
		}
	}
	
	public void SetPolicy(Policy policy)
	{
		RegistryKey registryKey = GetRegistryKey(policy.RegistryKeyType);
		RegistryValueKind valueKind = GetRegistryValueKind(policy.RegistryValueKind);

		using var targetKey = registryKey.CreateSubKey(policy.RegistryPath, true);
		if (targetKey is null)
			throw new Exception($"Could not open or create key: {policy.RegistryPath}");
		
		var registryValue = GetRegistryValue(policy.RegistryValue);
		if (registryValue is null)
			throw new ArgumentNullException(nameof(policy.RegistryValue), "Registry value cannot be null");
		
		targetKey.SetValue(policy.RegistryKey, registryValue, valueKind);
	}
	
	public Task ChangePolicyWatcher(Action<string, string> onPolicyChanged, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	
	private Microsoft.Win32.RegistryKey GetRegistryKey(Domain.Models.RegistryKeyType grpcRegistryKey)
	{
		return grpcRegistryKey switch
		{
			Domain.Models.RegistryKeyType.Hkcr => Microsoft.Win32.Registry.ClassesRoot,
			Domain.Models.RegistryKeyType.Hkcu => Microsoft.Win32.Registry.CurrentUser,
			Domain.Models.RegistryKeyType.Hklm => Microsoft.Win32.Registry.LocalMachine,
			Domain.Models.RegistryKeyType.Hkus => Microsoft.Win32.Registry.Users,
			_ => throw new ArgumentOutOfRangeException(nameof(grpcRegistryKey), grpcRegistryKey, null)
		};
	}
	
	private Microsoft.Win32.RegistryValueKind GetRegistryValueKind(Domain.Models.RegistryValueKind registryValueKind)
	{
		return registryValueKind switch
		{
			Domain.Models.RegistryValueKind.String => Microsoft.Win32.RegistryValueKind.String,
			Domain.Models.RegistryValueKind.Binary => Microsoft.Win32.RegistryValueKind.Binary,
			Domain.Models.RegistryValueKind.DWord => Microsoft.Win32.RegistryValueKind.DWord,
			Domain.Models.RegistryValueKind.QWord => Microsoft.Win32.RegistryValueKind.QWord,
			_ => throw new ArgumentOutOfRangeException(nameof(registryValueKind), registryValueKind, null)
		};
	}
	
	private object? GetRegistryValue(object registerValue)
	{
		return registerValue switch
		{
			int intValue => intValue,
			long longValue => longValue,
			uint uintValue => uintValue,
			string stringValue => stringValue,
			byte[] byteArray => byteArray,
			_ => throw new ArgumentOutOfRangeException(nameof(registerValue), registerValue, "Unsupported registry value type")
		};
	}
}