using DnsClient.Internal;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Entities.Queue.Mongo.Policy;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Shared.Constants;

namespace Messaging.Domain.Data.Policy.Commands.CreatePolicy;

public class CreatePolicyCommandHandler(
    IMongoClient mongo,
    ILogger<CreatePolicyCommandHandler> logger
)
{
    private const string POLICY_COLLECITON_NAME = "queue_policy";

    private readonly IMongoCollection<QueuePolicyDocument> collection = mongo
        .GetDatabase(MongoDbConfiguration.QUEUES_DB)
        .GetCollection<QueuePolicyDocument>(POLICY_COLLECITON_NAME);

    public async Task<bool> Handle(CreatePolicyCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var queueDocument = QueuePolicyDocument.CreateInstance("test");
            await collection.InsertOneAsync(queueDocument, cancellationToken: cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("{message}", ex.Message);
            return false;
        }
    }
}
