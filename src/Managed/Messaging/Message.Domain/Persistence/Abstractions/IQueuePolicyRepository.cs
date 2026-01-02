using Colombo.ResultPattern;
using Messaging.Domain.Entities.Queue.Mongo.Policy;

namespace Messaging.Domain.Persistence.Abstractions;

public interface IQueuePolicyRepository
{
    Task<Result<List<QueuePolicyDocument>>> Get(CancellationToken cancellationToken = default);
    Task<Result<QueuePolicyDocument>> Get(string queueName, CancellationToken cancellationToken = default);
    Task<Result> CreatePolicy(QueuePolicyDocument document, CancellationToken cancellationToken = default);
    Task<Result> UpdatePolicy(QueuePolicyDocument document, CancellationToken cancellationToken = default);
}
