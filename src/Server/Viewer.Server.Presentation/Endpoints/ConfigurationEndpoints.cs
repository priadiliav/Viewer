using Microsoft.AspNetCore.Mvc;
using Viewer.Server.Application.Dtos;
using Viewer.Server.Application.Services;

namespace Viewer.Server.Presentation.Endpoints;

public static class ConfigurationEndpoints
{
	private static async Task<IEnumerable<ConfigurationDto>> GetConfigurations(IConfigurationService configurationService)
	{
		var configurations = await configurationService.GetAllAsync();
		return configurations;
	}
	
    private static async Task<ConfigurationDetailsDto?> CreateConfiguration(IConfigurationService configurationService, 
			[FromBody] ConfigurationCreateRequest createRequest)
	{
		var configuration = await configurationService.CreateAsync(createRequest);
		return configuration;
	}
	
    private static async Task<ConfigurationDetailsDto> UpdateConfiguration(IConfigurationService configurationService, 
			[FromBody] ConfigurationUpdateRequest updateRequest, long id)
	{
		var configuration = await configurationService.UpdateAsync(id, updateRequest);
		return configuration;
	}
	
    private static async Task<ConfigurationDetailsDto?> GetConfigurationById(IConfigurationService configurationService, long id)
	{
		var configuration = await configurationService.GetByIdAsync(id);
		return configuration;
	}

    private static async Task DeleteConfiguration(IConfigurationService configurationService, long id)
    {
        await configurationService.DeleteAsync(id);
    }
    
    private static async Task ApplyConfiguration(IConfigurationService configurationService, long id)
    {
        await configurationService.ApplyConfiguration(id);
    }
	
	public static void MapConfigurationEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/api/configurations").WithTags("Configurations");
		
		group.MapGet("/", GetConfigurations).WithName("GetConfigurations")
			.Produces<IEnumerable<ConfigurationDto>>()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.WithSummary("Get all configurations");
		
		group.MapGet("/{id:long}", GetConfigurationById).WithName("GetConfigurationById")
			.Produces<ConfigurationDetailsDto?>()
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status200OK)
			.WithSummary("Get configuration by ID");
		
		group.MapPost("/", CreateConfiguration).WithName("CreateConfiguration")
			.Produces<ConfigurationDetailsDto>()
			.Produces(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.WithSummary("Create a new configuration")
			.Accepts<ConfigurationCreateRequest>("application/json");
		
		group.MapPut("/{id:long}", UpdateConfiguration).WithName("UpdateConfiguration")
			.Produces<ConfigurationDetailsDto>()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status400BadRequest)
			.WithSummary("Update an existing configuration")
			.Accepts<ConfigurationUpdateRequest>("application/json");
        
        group.MapDelete("/{id:long}", DeleteConfiguration).WithName("DeleteConfiguration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Delete a configuration by ID");
        
        group.MapPost("/{id:long}/apply", ApplyConfiguration).WithName("ApplyConfiguration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Apply a configuration by ID");
	}
}
