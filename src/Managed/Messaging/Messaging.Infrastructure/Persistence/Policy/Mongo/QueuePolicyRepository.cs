using Messaging.Domain.Data.Policy.Persistence.Abstractions;
using Messaging.Domain.Entities.Queue.Mongo.Policy;
using MongoDB.Driver;
using Shared.Constants;

namespace Messaging.Infrastructure.Persistence.Policy.Mongo;

public class QueuePolicyRepository(IMongoClient mongodb) : IQueuePolicyRepository
{
    private const string POLICY_COLLECITON_NAME = "queue_policy";

    private readonly IMongoCollection<QueuePolicyDocument> collection = mongodb
        .GetDatabase(MongoDbConfiguration.QUEUES_DB)
        .GetCollection<QueuePolicyDocument>(POLICY_COLLECITON_NAME);

    public async Task<bool> CreatePolicy(QueuePolicyDocument document, CancellationToken cancellationToken = default)
    {
        await collection.InsertOneAsync(document, cancellationToken: cancellationToken);
        return true;
    }
}
