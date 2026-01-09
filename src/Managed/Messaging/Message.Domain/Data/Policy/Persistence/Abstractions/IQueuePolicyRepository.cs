using Package.ResultPattern;
using Messaging.Domain.Data.Policy.Enums;
using Messaging.Domain.Entities.Queue.Mongo.Policy;

namespace Messaging.Domain.Data.Policy.Persistence.Abstractions;

public interface IQueuePolicyRepository
{
    Task<Result<List<QueuePolicyDocument>>> Get(
        FilterQueuePolicy filter = FilterQueuePolicy.None,
        CancellationToken cancellationToken = default);

    Task<Result<QueuePolicyDocument>> Get(string queueName, CancellationToken cancellationToken = default);
    Task<Result<List<QueuePolicyDocument>>> Get(CancellationToken cancellationToken = default, params string[] queues);
    Task<Result> CreatePolicy(QueuePolicyDocument document, CancellationToken cancellationToken = default);
    Task<Result> UpdatePolicy(QueuePolicyDocument document, CancellationToken cancellationToken = default);
}
