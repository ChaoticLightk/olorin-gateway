using Gateway.Domain.Repositories.Config.Interfaces;
using Gateway.Infrastructure.Repositories.Config;
using Gateway.Providers;
using Gateway.Providers.Interfaces;
using Gateway.Shared.Constants.Database;
using Microsoft.AspNetCore.Mvc;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddMongoDBClient(MongoDbConfiguration.DB_NAME);

builder.Services.AddSingleton<IConfigRepository, OlorinConfigRepository>();
builder.Services.AddSingleton<IProxyConfigProvider, OlorinConfigProvider>();

builder.Services.AddReverseProxy();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

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