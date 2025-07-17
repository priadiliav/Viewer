using Microsoft.AspNetCore.Mvc;
using Viewer.Server.Application.Dtos.Policy;
using Viewer.Server.Application.Interfaces.Services;
using Viewer.Server.Application.Services;

namespace Viewer.Server.Presentation.Endpoints;

public static class PolicyEndpoints
{
	private static async Task<IEnumerable<PolicyDto>> GetPolicies(IPolicyService policyService)
	{
		var policies = await policyService.GetAllAsync();
		return policies;
	}
	
    private static async Task<PolicyDto?> GetPolicyById(IPolicyService policyService, long id)
	{
		var policy = await policyService.GetByIdAsync(id);
		return policy;
	}
	
    private static async Task<PolicyDto?> CreatePolicy(IPolicyService policyService, PolicyCreateRequest createRequest)
	{
		var policy = await policyService.CreateAsync(createRequest);
		return policy;
	}

    private static async Task<PolicyDto?> UpdatePolicy(IPolicyService policyService, long id,
            [FromBody] PolicyUpdateRequest updateRequest)
    {
        var policy = await policyService.UpdateAsync(id, updateRequest);
        return policy;
    }
    
    private static async Task DeletePolicy(IPolicyService policyService, long id)
    {
        await policyService.DeleteAsync(id);
    }
	
	public static void MapPolicyEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/api/policies").WithTags("Policies");
		
		group.MapGet("/", GetPolicies).WithName("GetPolicies")
			.Produces<IEnumerable<PolicyDto>>()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.WithSummary("Get all policies");
		
		group.MapGet("/{id:long}", GetPolicyById).WithName("GetPolicyById")
			.Produces<PolicyDto?>()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.WithSummary("Get policy by ID");

		group.MapPost("/", CreatePolicy).WithName("CreatePolicy")
			.Produces<PolicyDto>()
			.Produces(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.WithSummary("Create a new policy")
			.Accepts<PolicyCreateRequest>("application/json");
        
        group.MapPut("/{id:long}", UpdatePolicy).WithName("UpdatePolicy")
            .Produces<PolicyDto?>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Update an existing policy")
            .Accepts<PolicyUpdateRequest>("application/json");
        
        group.MapDelete("/{id:long}", DeletePolicy).WithName("DeletePolicy")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Delete a policy");
	}
}
