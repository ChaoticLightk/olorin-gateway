using Messaging.Domain.Entities.Queue.Mongo.Policy;
using Messaging.Domain.Shared.Messaging.DTO;

namespace Messaging.Domain.Shared.Messaging.Services;

public interface IQueueService
{
    Task PublishAsync(
        object? content,
        string routingKey,
        string exchange = "",
        MessagingMetadata? metadata = null,
        bool mandatory = false,
        CancellationToken ct = default);

    Task PublishAsync(
        string message,
        string routingKey,
        string exchange = "",
        MessagingMetadata? metadata = null,
        bool mandatory = false,
        CancellationToken ct = default);
    
    Task ProvisionAsync(
        QueuePolicyDocument policy,
        CancellationToken ct
    );
}
