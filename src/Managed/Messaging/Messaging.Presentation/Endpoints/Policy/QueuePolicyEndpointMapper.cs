using Extensions.Endpoints.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messaging.Presentation.Endpoints.Policy;

public class QueuePolicyEndpointMapper : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/messaging/policy")
            .MapQueuePolicyEndpoints()
            .WithTags("Queue", "Messaging", "Policy");
    }
}

