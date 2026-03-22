using Extensions.Endpoints;
using Gateway.DependecyInjection;
using Gateway.Domain;
using Gateway.Extensions.YARP.Response.Transforms.Providers;
using Gateway.Infrastructure;
using Gateway.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services
    .AddReverseProxy()
    .AddTransforms<BodyResponseTransformProvider>();

builder.AddServiceDefaults();

builder.AddCorsModules();
builder.AddAuthenticationModule();
builder.AddDependencies();

builder.AddDomain();
builder.AddInfrastructure();

var app = builder.Build();

app.MapOpenApi();
app.MapReverseProxy();
app.MapDefaultEndpoints();
app.UseHttpsRedirection();

app.ConfigureAuthenticationModule();
app.ConfigurePresentation();
app.ConfigureCorsModule();
app.ConfigureScalar();

app.Run();