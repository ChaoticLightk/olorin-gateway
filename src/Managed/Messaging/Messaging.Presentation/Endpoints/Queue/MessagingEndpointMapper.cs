using Extensions.Endpoints.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messaging.Presentation.Endpoints.Queue;

public class MessagingEndpointMapper : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGroup("/messaging/queue")
            .MapMessagingEndpoints()
            .WithTags("Queue");
    }
}