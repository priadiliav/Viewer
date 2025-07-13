using Viewer.Server.Application.Dtos.Policy;
using Viewer.Server.Application.Services;

namespace Viewer.Server.Presentation.Endpoints;

public static class PolicyEndpoints
{
	public static async Task<IEnumerable<PolicyDto>> GetPolicies(IPolicyService policyService)
	{
		var policies = await policyService.GetAllAsync();
		return policies;
	}
	
	public static async Task<PolicyDto?> GetPolicyById(IPolicyService policyService, long id)
	{
		var policy = await policyService.GetByIdAsync(id);
		return policy;
	}
	
	public static async Task<PolicyDto> CreatePolicy(IPolicyService policyService, PolicyCreateRequest createRequest)
	{
		var policy = await policyService.CreateAsync(createRequest);
		return policy;
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
	}
}