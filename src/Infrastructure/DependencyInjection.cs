using Domain.Repositories.Interfaces;
using Domain.Shared.Constants;
using Infrastructure.Repositories.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddMongoDBClient(MongoDbConfiguration.DB_NAME);

        builder.Services.AddSingleton<IConfigRepository, OlorinEndpointsConfigRepository>();

        return builder;
    }
}
