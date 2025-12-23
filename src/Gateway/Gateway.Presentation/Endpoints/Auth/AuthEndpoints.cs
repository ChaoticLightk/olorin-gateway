using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Extensions.Endpoints.Abstractions;
using Gateway.Domain.Data.Auth.DTO;
using MiddleR.Abstractions;
using Gateway.Domain.Data.Auth.Command;

namespace Gateway.Presentation.Endpoints.Auth;

public class AuthEndpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/authorize", static async (
            [FromServices] IServiceBus service,
            [FromBody] AuthorizeRequest request) =>
        {
            var result = await service.Send(new AuthorizeApplicationCommand(
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
