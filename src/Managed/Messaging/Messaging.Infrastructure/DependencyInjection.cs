using Messaging.Domain.Data.Messaging.Persistence.Abstractions;
using Messaging.Domain.Data.Policy.Persistence.Abstractions;
using Messaging.Domain.Shared.Messaging.Services;
using Messaging.Infrastructure.Persistence.Messaging.Mongo;
using Messaging.Infrastructure.Persistence.Policy.Mongo;
using Messaging.Infrastructure.Shared.Messaging.Services;
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

        builder.Services.AddSingleton<IQueueService, QueueService>();
        builder.Services.AddScoped<IQueuePolicyRepository, QueuePolicyRepository>();
        builder.Services.AddScoped<IQueueMessageRepository, QueueMessageRepository>();

        return builder;
    }
}
