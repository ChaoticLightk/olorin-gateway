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
        Message message, 
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
}
