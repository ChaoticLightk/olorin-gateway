using Messaging.Domain.Data.Policy.Persistence.Abstractions;
using Messaging.Infrastructure.Persistence.Policy.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Constants;

namespace Messaging.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddRabbitMQClient(RabbitMQConfiguration.CONNECTION_NAME);
        builder.AddMongoDBClient(MongoDbConfiguration.QUEUES_DB);

        builder.Services.AddScoped<IQueuePolicyRepository, QueuePolicyRepository>();

        return builder;
    }
}
