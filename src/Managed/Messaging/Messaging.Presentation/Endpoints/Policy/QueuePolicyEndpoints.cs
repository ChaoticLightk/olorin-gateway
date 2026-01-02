using Extensions.Endpoints.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Messaging.Presentation.Endpoints.Policy;

public partial class QueuePolicyEndpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/messaging/policy", CreatePolicy);
        app.MapGet("/messaging/policy", GetAllPolicies);
        app.MapPatch("/messaging/policy", PatchPolicyById);
        app.MapGet("/messaging/policy/{id}", GetPolicyById);
        app.MapDelete("/messaging/policy/{id}", DeletePolicyById);
    }
}

