using Colombo.ResultPattern;
using Messaging.Domain.Data.Messaging.Persistence.Abstractions;
using Messaging.Domain.Entities.Queue.Mongo;
using Messaging.Infrastructure.Persistence.Abstractions.Mongo;
using MongoDB.Driver;
using Shared.Constants;

namespace Messaging.Infrastructure.Persistence.Messaging.Mongo;

public class QueueMessageRepository(IMongoClient mongodb) 
    : MongoBaseRepository, IQueueMessageRepository
{
    private const string MESSAGE_COLLECTION_NAME = "queue_messages";

    private readonly IMongoCollection<MessageDocument> collection = mongodb
        .GetDatabase(MongoDbConfiguration.QUEUES_DB)
        .GetCollection<MessageDocument>(MESSAGE_COLLECTION_NAME);

    public async Task<Result<MessageDocument>> CreateMessage(
        MessageDocument document,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await collection.InsertOneAsync(document, cancellationToken: cancellationToken);
            return document;
        }, "Não foi possivel inserir politica de filas");
    }

    public async Task<Result> UpdateMessage(MessageDocument document, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(() => collection
            .FindOneAndReplaceAsync(
                x => x.Id.Equals(document.Id),
                document,
                cancellationToken: cancellationToken)
            , "Não foi possivel atualizar a mensage.");
    }
}
