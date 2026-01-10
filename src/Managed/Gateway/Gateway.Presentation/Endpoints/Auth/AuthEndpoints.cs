using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Extensions.Endpoints.Abstractions;
using Gateway.Domain.Data.Auth.DTO;
using Gateway.Domain.Data.Auth.Command;
using Wolverine;

namespace Gateway.Presentation.Endpoints.Auth;

public class AuthEndpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/authorize", static async (
            [FromServices] IMessageBus bus,
            [FromBody] AuthorizeRequest request) =>
        {
            var result = await bus.InvokeAsync<string?>(new AuthorizeApplicationCommand(
                request.Application, 
                request.Password));

            if (result is not null)
            {
                return Results.Ok(new
                {
                    token = result
                });
            }

            return Results.Unauthorized();
        });
    }
}
