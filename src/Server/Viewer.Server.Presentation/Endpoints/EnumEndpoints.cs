using Viewer.Server.Domain.Models;

namespace Viewer.Server.Presentation.Endpoints;

public static class EnumEndpoints
{
    public static void MapEnumEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/enums").WithTags("Enums");

        group.MapGet("", () =>
        {
            var processStatuses = Enum.GetValues(typeof(ProcessStatus))
                .Cast<ProcessStatus>()
                .Select(e => new { Value = (int)e, Key = e.ToString() })
                .ToList();
            
            var registryTypes = Enum.GetValues(typeof(RegistryType))
                .Cast<RegistryType>()
                .Select(e => new { Value = (int)e, Key = e.ToString() })
                .ToList();
            
            var registryKeyTypes = Enum.GetValues(typeof(RegistryKeyType))
                .Cast<RegistryKeyType>()
                .Select(e => new { Value = (int)e, Key = e.ToString() })
                .ToList();
            
            return Results.Ok(new
            {
                ProcessStatuses = processStatuses,
                RegistryTypes = registryTypes,
                RegistryKeyTypes = registryKeyTypes
            });
        });
    }
}
