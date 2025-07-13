using Viewer.Agent.Domain.Models;

namespace Viewer.Agent.Application.Interfaces.Repositories;

public interface IPolicyRepository
{
	List<Policy> GetConfigPolicies();
	void SaveConfigPolicies(List<Policy> policies);
	void SetPolicies(List<Policy>? policies);
	void SetPolicy(Policy policy);
}