using System;
using Gateway.Application.Authentication.Services;
using Gateway.Domain.Configuration.Jwt;
using Gateway.Domain.Data.Auth.Services.Interfaces;
using Gateway.Domain.Repositories.Interfaces;
using Gateway.Extensions.YARP.LoadBalancing;
using Gateway.Infrastructure.Repositories;
using Gateway.Providers;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

namespace Gateway.DependecyInjection;

public static class Dependencies
{
    public static WebApplicationBuilder AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection(JWTSettings.SECTION_NAME));

        builder.Services.AddSingleton<IProxyConfigProvider, MongoConfigProvider>();
        builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddSingleton<ILoadBalancingPolicy, WeightedLoadBalancingPolicy>();
        builder.Services.AddSingleton<ILoadBalancingPolicy, EnabledAwareLoadBalancingPolicy>();

        return builder;
    }
}
