using Domain.Repositories.Interfaces;
using Gateway.Application.Authentication.DTO;
using Gateway.Application.Authentication.Services;
using Gateway.Application.Authentication.Services.Interfaces;
using Gateway.Configuration.Jwt;
using Gateway.DependecyInjection;
using Gateway.Extensions.YARP.LoadBalancing;
using Gateway.Extensions.YARP.Response.Transforms.Providers;
using Gateway.Providers;
using Gateway.Providers.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection(JWTSettings.SECTION_NAME));

builder.AddCorsModules();
builder.AddAuthenticationModule();

builder.AddInfrastructure();

builder.Services.AddSingleton<IProxyConfigProvider, OlorinConfigProvider>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddSingleton<ILoadBalancingPolicy, WeightedLoadBalancingPolicy>();
builder.Services.AddSingleton<ILoadBalancingPolicy, EnabledAwareLoadBalancingPolicy>();

builder.Services
    .AddReverseProxy()
    .AddTransforms<BodyResponseTransformProvider>();

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

    if (result is not null)
    {
        return Results.Ok(new
        {
            token = result
        });
    }

    return Results.Unauthorized();
});

app.Run();