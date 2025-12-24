using Gateway.Domain.Repositories.Interfaces;
using Gateway.Infrastructure.Repositories.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Constants;

namespace Gateway.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddMongoDBClient(MongoDbConfiguration.PROXY_DB);

        builder.Services.AddSingleton<IConfigRepository, OlorinEndpointsConfigRepository>();

        return builder;
    }
}
