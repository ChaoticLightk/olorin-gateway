using Package.ResultPattern;
using Messaging.Domain.Data.Messaging.Persistence.Abstractions;
using Messaging.Domain.Entities.Queue.Mongo;
using Messaging.Infrastructure.Persistence.Abstractions.Mongo;
using MongoDB.Driver;
using Shared.Constants;
using Messaging.Domain.Shared.Messaging.DTO;

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

    public async Task<Result<List<MessageDocument>>> Get(string queueName = "", CancellationToken cancellationToken = default)
    {
        var filter = Builders<MessageDocument>.Filter.Empty;

        if (!string.IsNullOrEmpty(queueName))
        {
            filter = Builders<MessageDocument>.Filter
                .Where(x => x.Queue.Equals(queueName));
        }

        return await ExecuteAsync(() => collection
            .Find(filter)
            .ToListAsync(cancellationToken: cancellationToken)
        , "Não foi possivel buscar as mensagens");
    }

    public async Task<Result<MessageDocument>> GetById(string id, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(() => collection
            .Find(Builders<MessageDocument>.Filter
                .Eq(u => u.Id, id))
            .FirstOrDefaultAsync(cancellationToken)
        , "Não foi possivel buscar a mensagem.");   
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
