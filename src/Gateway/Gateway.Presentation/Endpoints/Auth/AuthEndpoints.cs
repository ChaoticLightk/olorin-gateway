using Domain.Data.Auth.DTO;
using Domain.Data.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;

namespace Presentation.Endpoints.Auth;

public class AuthEndpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/authorize", static async (
            [FromServices] IAuthService service,
            [FromBody] AuthorizeRequest request) =>
        {
            var result = await service.AuthorizeApplication(
                request.Application,
                request.Password);

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
