using Gateway.Domain.Repositories.Config.Interfaces;
using Gateway.Infrastructure.Repositories.Config;
using Gateway.Providers;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfigRepository, OlorinConfigRepository>();
builder.Services.AddSingleton<IProxyConfigProvider, OlorinConfigProvider>();

builder.Services.AddOpenApi();

builder.Services.AddReverseProxy();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapReverseProxy();
app.Run();