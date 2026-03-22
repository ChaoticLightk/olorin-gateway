using Messaging.Domain.Entities.Queue.Mongo.Policy;
using Messaging.Domain.Shared.Messaging.DTO;

namespace Messaging.Domain.Shared.Messaging.Mappers;

public static class ProvisionQueueMapper
{
    public static ProvisionQueue MapFromDocument(QueuePolicyDocument document)
    {
        return document.QueueType switch
        {
            QueueType.Quorum => ProvisionQueue.Quorum(
                document.QueueName,
                document.RetryPolicy is not null,
                document.RetryPolicy?.MaxRetries ?? 0,
                document.Enabled,
                document.EnabledDeadLetter
            ),
            QueueType.Stream => ProvisionQueue.Stream(
                document.QueueName,
                document.Enabled,
                document.EnabledDeadLetter
            ),
            QueueType.Classic or _ => ProvisionQueue.Classic(
                document.QueueName,
                document.Enabled,
                document.EnabledDeadLetter
            ),
        };
    }
}
