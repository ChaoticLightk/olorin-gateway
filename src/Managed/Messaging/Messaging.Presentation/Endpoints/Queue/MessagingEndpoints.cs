using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Messaging.Presentation.Endpoints.Queue;

public static partial class MessagingEndpoints
{
    public static RouteGroupBuilder MapMessagingEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/", PublishMessage);

        group.MapGet("/", GetMessages);
        group.MapGet("/{id}", GetMessageById);
        group.MapGet("/queue/{queue}", GetMessagesByQueue);

        group.MapPatch("/edit/{id}", PatchMessage);

        group.MapPut("/retry", RetryMessages);

        group.MapDelete("/delete", DeleteMessages);

        return group;
    }
}
