using Extensions.Endpoints.Abstractions;
using Gateway.Domain.Providers.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Presentation.Endpoints.Proxy;

public class ProxyEndpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/refresh", ([FromServices] IProxyConfigProvider provider) =>
        {
            if (provider is not IReloadableProxyConfigProvider reloadableProvider)
            {
                return Results.BadRequest(new { message = "Provider does not support reload." });
            }

            reloadableProvider.Reload();

            return Results.Ok(new { message = "Reverse proxy configuration refreshed." });
        });
    }
}
