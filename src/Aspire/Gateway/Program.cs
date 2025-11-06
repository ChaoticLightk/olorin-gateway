using Domain.Shared.Constants;
using Gateway.DependecyInjection;
using Gateway.Providers;
using Gateway.Providers.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddCorsModules();
builder.AddAuthenticationModule();

builder.AddInfrastructure();

builder.Services.AddSingleton<IProxyConfigProvider, OlorinConfigProvider>();

builder.Services.AddReverseProxy();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.ConfigureAuthentication();
app.ConfigureCorsPolicy();

app.MapReverseProxy();

app.MapPost("/refresh", ([FromServices]IProxyConfigProvider provider) =>
{
    if (provider is IReloadableProxyConfigProvider reloadableProvider)
    {
        reloadableProvider.Reload();
        return Results.Ok(new { message = "Reverse proxy configuration refreshed." });
    }

    return Results.BadRequest(new { message = "Provider does not support reload." });
});

app.Run();