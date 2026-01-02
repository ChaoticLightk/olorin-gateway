using System.Collections.ObjectModel;
using Colombo.ResultPattern;
using Colombo.ResultPattern.ErrorResult;
using Messaging.Domain.Entities.Queue.Mongo.Policy;
using Messaging.Domain.Persistence.Abstractions;
using MongoDB.Driver;
using Shared.Constants;

namespace Messaging.Infrastructure.Persistence.Policy.Mongo;

public class QueuePolicyRepository(IMongoClient mongodb) : IQueuePolicyRepository
{
    private const string POLICY_COLLECITON_NAME = "queue_policy";

    private readonly IMongoCollection<QueuePolicyDocument> collection = mongodb
        .GetDatabase(MongoDbConfiguration.QUEUES_DB)
        .GetCollection<QueuePolicyDocument>(POLICY_COLLECITON_NAME);

    public async Task<Result<List<QueuePolicyDocument>>> Get(CancellationToken cancellationToken = default)
    {
        try
        {
            return await collection
                .Find(_ => true)
                .ToListAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            return Error.InternalServer("Não foi possivel buscar as politicas de filas", ex: ex);
        }
    }

    public async Task<Result<QueuePolicyDocument>> Get(string policy, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<QueuePolicyDocument>
                .Filter.Eq(u => u.QueueName, policy);

            return await collection
                .Find(filter)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Error.InternalServer("Não foi possivel buscar as politicas de filas", ex: ex);
        }
    }

    public async Task<Result> CreatePolicy(QueuePolicyDocument document, CancellationToken cancellationToken)
    {
        try
        {
            await collection.InsertOneAsync(document, cancellationToken: cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Error.InternalServer("Não foi possivel inserir politica de filas", ex: ex);
        }
    }

    public async Task<Result> UpdatePolicy(QueuePolicyDocument document, CancellationToken cancellationToken)
    {
        try
        {
            await collection.FindOneAndReplaceAsync(
                x => x.QueueName.Equals(document.QueueName), 
                document,
                cancellationToken: cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Error.InternalServer("Não foi possivel atualizar politica de filas", ex: ex);
        }
    }
}
