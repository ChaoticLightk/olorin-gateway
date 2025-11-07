using Domain.Repositories.Interfaces;
using Domain.Shared.Constants;
using Gateway.Application.Authentication.DTO;
using Gateway.Application.Authentication.Services;
using Gateway.Application.Authentication.Services.Interfaces;
using Gateway.DependecyInjection;
using Gateway.Providers;
using Gateway.Providers.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddCorsModules();
builder.AddAuthenticationModule();

builder.AddInfrastructure();

builder.Services.AddSingleton<IProxyConfigProvider, OlorinConfigProvider>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddReverseProxy();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.ConfigureAuthenticationModule();
app.ConfigureCorsModule();

app.MapReverseProxy();

var api = app.MapGroup("/api");

api.MapPost("/refresh", ([FromServices] IProxyConfigProvider provider) =>
{
    if (provider is IReloadableProxyConfigProvider reloadableProvider)
    {
        reloadableProvider.Reload();
        return Results.Ok(new { message = "Reverse proxy configuration refreshed." });
    }

    return Results.BadRequest(new { message = "Provider does not support reload." });
});

api.MapPost("/authorize", static async (
    [FromServices] IAuthenticationService service,
    [FromBody] AuthorizeRequest request) =>
{
    var result = await service.AuthorizeApplication(
        request.Application,
        request.Password);

    if (result)
    {
        return Results.Ok();
    }

    return Results.Unauthorized();
});

app.Run();