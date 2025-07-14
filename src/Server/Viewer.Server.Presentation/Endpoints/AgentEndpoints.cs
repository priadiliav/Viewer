using Microsoft.AspNetCore.Mvc;
using Viewer.Server.Application.Dtos.Agent;
using Viewer.Server.Application.Services;

namespace Viewer.Server.Presentation.Endpoints;

public static class AgentEndpoints
{
	private static async Task<IEnumerable<AgentDto>> GetAgents(IAgentService agentService)
	{
		var agents = await agentService.GetAllAsync();
		return agents;
	}
	
	private static async Task<AgentDetailsDto?> CreateAgent(IAgentService agentService, [FromBody] AgentCreateRequest createRequest)
	{
		var agent = await agentService.CreateAsync(createRequest);
		return agent;
	}

	private static async Task<AgentDetailsDto?> GetAgentById(IAgentService agentService, Guid id)
	{
		var agent = await agentService.GetByIdAsync(id);
		return agent;
	}

    private static async Task<AgentDetailsDto?> UpdateAgent(IAgentService agentService, Guid id, [FromBody] AgentUpdateRequest updateRequest)
    {
        var agent = await agentService.UpdateAsync(id, updateRequest);
        return agent;
    }
    
    private static async Task DeleteAgent(IAgentService agentService, Guid id)
    {
        await agentService.DeleteAsync(id);
    }
    
	public static void MapAgentEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/api/agents").WithTags("Agents");
		
		group.MapGet("/", GetAgents).WithName("GetAgents")
			.Produces<IEnumerable<AgentDto>>()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.WithSummary("Get all agents");
		
		group.MapGet("/{id:guid}", GetAgentById).WithName("GetAgentById")
			.Produces<AgentDetailsDto?>()
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status200OK)
			.WithSummary("Get agent by ID");

		group.MapPost("/", CreateAgent).WithName("CreateAgent")
			.Produces<AgentDetailsDto>()
			.WithSummary("Create a new agent")
			.Produces(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.Accepts<AgentCreateRequest>("application/json");
        
        group.MapPut("/{id:guid}", UpdateAgent).WithName("UpdateAgent")
            .Produces<AgentDetailsDto?>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status200OK)
            .WithSummary("Update an existing agent")
            .Accepts<AgentUpdateRequest>("application/json");
        
        group.MapDelete("/{id:guid}", DeleteAgent).WithName("DeleteAgent")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Delete an agent");
	}
}
