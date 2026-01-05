using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Messaging.Presentation.Endpoints.Policy;

public static partial class QueuePolicyEndpoints
{
    public static RouteGroupBuilder MapQueuePolicyEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/", CreatePolicy);
        group.MapGet("/", GetAllPolicies);
        group.MapPatch("/", PatchPolicy);
        group.MapGet("/{id}", GetPolicyById);
        group.MapDelete("/{id}", DeletePolicyById);
        group.MapPost("/provision", ProvisionPolicy);

        return group;
    }
}

