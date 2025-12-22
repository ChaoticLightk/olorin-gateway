using Domain.Data.Auth.Services.Interfaces;
using Domain.Repositories.Interfaces;
using Gateway.Application.Authentication.Services;
using Gateway.Configuration.Jwt;
using Gateway.DependecyInjection;
using Gateway.Extensions.YARP.LoadBalancing;
using Gateway.Extensions.YARP.Response.Transforms.Providers;
using Gateway.Providers;
using Infrastructure;
using Infrastructure.Repositories;
using Presentation;
using Scalar.AspNetCore;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection(JWTSettings.SECTION_NAME));

builder.Services.AddOpenApi();

builder.AddCorsModules();
builder.AddAuthenticationModule();

builder.AddInfrastructure();

builder.Services.AddSingleton<IProxyConfigProvider, OlorinConfigProvider>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IAuthService, AuthenticationService>();

builder.Services.AddSingleton<ILoadBalancingPolicy, WeightedLoadBalancingPolicy>();
builder.Services.AddSingleton<ILoadBalancingPolicy, EnabledAwareLoadBalancingPolicy>();

builder.Services
    .AddReverseProxy()
    .AddTransforms<BodyResponseTransformProvider>();

var app = builder.Build();

app.MapOpenApi();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.ConfigureAuthenticationModule();
app.ConfigureCorsModule();
app.AddPresentation();

app.MapReverseProxy();

app.MapScalarApiReference();

app.Run();