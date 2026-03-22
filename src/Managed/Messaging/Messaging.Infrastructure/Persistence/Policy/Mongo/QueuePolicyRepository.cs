using Package.ResultPattern;
using Messaging.Domain.Data.Policy.Enums;
using Messaging.Domain.Data.Policy.Persistence.Abstractions;
using Messaging.Domain.Entities.Queue.Mongo.Policy;
using Messaging.Infrastructure.Persistence.Abstractions.Mongo;
using MongoDB.Driver;
using Shared.Constants;

namespace Messaging.Infrastructure.Persistence.Policy.Mongo;

public class QueuePolicyRepository(IMongoClient mongodb)
    : MongoBaseRepository, IQueuePolicyRepository
{
    private const string POLICY_COLLECITON_NAME = "queue_policy";

    private readonly IMongoCollection<QueuePolicyDocument> collection = mongodb
        .GetDatabase(MongoDbConfiguration.QUEUES_DB)
        .GetCollection<QueuePolicyDocument>(POLICY_COLLECITON_NAME);

    public async Task<Result<List<QueuePolicyDocument>>> Get(
        FilterQueuePolicy filter = FilterQueuePolicy.None,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(() =>
        {
            var filterDef = filter switch
            {
                FilterQueuePolicy.IncludeDelete =>
                    Builders<QueuePolicyDocument>.Filter.Empty,
                FilterQueuePolicy.Onlydeleted =>
                    Builders<QueuePolicyDocument>.Filter
                        .Where(x => x.DeletedAt != null),
                FilterQueuePolicy.None or _ =>
                    Builders<QueuePolicyDocument>.Filter
                        .Where(x => x.DeletedAt == null)
            };

            return collection
                .Find(filterDef)
                .ToListAsync(cancellationToken: cancellationToken);
            }, "Não foi possivel buscar as politicas de filas");
    }

    public async Task<Result<QueuePolicyDocument>> Get(string policy, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(() => collection
            .Find(Builders<QueuePolicyDocument>.Filter
                .Eq(u => u.QueueName, policy))
            .FirstOrDefaultAsync(cancellationToken)
        , "Não foi possivel buscar as politicas de filas");
    }

    public async Task<Result<List<QueuePolicyDocument>>> Get(
        CancellationToken cancellationToken = default,
        params string[] queues)
    {
        return await ExecuteAsync(() => collection
            .Find(x => queues.Contains(x.QueueName))
            .ToListAsync(cancellationToken: cancellationToken)
        , "Não foi possivel buscar as politicas de filas");
    }

    public async Task<Result> CreatePolicy(QueuePolicyDocument document, CancellationToken cancellationToken)
    {
        return await ExecuteAsync(() => collection
            .InsertOneAsync(document, cancellationToken: cancellationToken)
        , "Não foi possivel inserir politica de filas");
    }

    public async Task<Result> UpdatePolicy(QueuePolicyDocument document, CancellationToken cancellationToken)
    {
        return await ExecuteAsync(() => collection
            .FindOneAndReplaceAsync(
                x => x.QueueName.Equals(document.QueueName),
                document,
                cancellationToken: cancellationToken)
            , "Não foi possivel atualizar politica de filas");
    }
}
