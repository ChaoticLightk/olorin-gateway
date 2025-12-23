using Gateway.Application.Authentication.Services;
using Gateway.DependecyInjection;
using Gateway.Domain;
using Gateway.Domain.Configuration.Jwt;
using Gateway.Domain.Data.Auth.Services.Interfaces;
using Gateway.Domain.Repositories.Interfaces;
using Gateway.Extensions.YARP.LoadBalancing;
using Gateway.Extensions.YARP.Response.Transforms.Providers;
using Gateway.Presentation;
using Gateway.Providers;
using Infrastructure;
using Infrastructure.Repositories;
using Scalar.AspNetCore;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection(JWTSettings.SECTION_NAME));

builder.Services.AddOpenApi();

builder.AddCorsModules();
builder.AddAuthenticationModule();

builder.AddDomain();
builder.AddInfrastructure();

builder.Services.AddSingleton<IProxyConfigProvider, OlorinConfigProvider>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

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