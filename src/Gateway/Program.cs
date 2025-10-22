using Gateway.Domain.Options;
using Gateway.Domain.Repositories.Config.Interfaces;
using Gateway.Infrastructure.Repositories.Config;
using Gateway.Providers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<MongoDbOptions>()
    .BindConfiguration(MongoDbOptions.SECTION_NAME)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<IMongoClient>(c =>
{
    var options = c.GetRequiredService<IOptions<MongoDbOptions>>().Value;
    return new MongoClient(options.ConnectionString);
});

builder.Services.AddSingleton<IConfigRepository, OlorinConfigRepository>();
builder.Services.AddSingleton<IProxyConfigProvider, OlorinConfigProvider>();

builder.Services.AddReverseProxy();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapReverseProxy();
app.Run();