using Messaging.Domain.Entities.Queue.Mongo.Policy;

namespace Messaging.Domain.Data.Policy.Persistence.Abstractions;

public interface IQueuePolicyRepository
{
    Task<bool> CreatePolicy(QueuePolicyDocument document, CancellationToken cancellationToken = default);
}
