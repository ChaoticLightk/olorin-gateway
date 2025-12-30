using Extensions.Endpoints.Abstractions;
using Messaging.Domain.Data.Policy.Commands.CreatePolicy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Messaging.Presentation.Endpoints.Policy;

public class QueuePolicyEndpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/messaging/policy", static async ([FromBody] CreatePolicyCommand request, IMessageBus bus) =>
        {
            var result = await bus.InvokeAsync<bool>(request);

            if (result)
            {
                return Results.Ok();
            }

            return Results.InternalServerError();
        });

        app.MapGet("/messaging/policy", static async () =>
        {
            return Results.BadRequest();
        });

        app.MapGet("/messaging/policy/{id}", static async (string id) =>
        {
            return Results.BadRequest();
        });

        app.MapPatch("/messaging/policy/{id}", static async (string id) =>
        {
            return Results.BadRequest();
        });

        app.MapDelete("/messaging/policy/{id}", static async (string id) =>
        {
            return Results.BadRequest();
        });
    }
}

