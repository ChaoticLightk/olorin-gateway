using System.Text.Json;
using Messaging.Domain.Entities.Queue.Enums;
using Messaging.Domain.Extensions;
using Messaging.Domain.Shared.Messaging.DTO;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo;

public class MessageDocument
{
    private MessageDocument(string queueName, int policyVersion, string correlationId, object content)
    {
        Queue = queueName;
        PolicyVersion = policyVersion;
        CreatedAt = DateTime.Now;
        CorrelationId = correlationId;
        State = MessageState.Created;
        Payload = content switch
        {
            JsonElement json => json.ParseToBsonDocument(),
            BsonDocument bson => bson,
            _ => content.ToBsonDocument()
        };
    }

    public static MessageDocument CreateInstance(
        string queueName,
        int policyVersion,
        string correlationId,
        object content)
    {
        return new MessageDocument(queueName, policyVersion, correlationId, content);
    }
    
    public void SetState(MessageState state)
    {
        State = state;
    }

    public string[] FieldsToIgnore => [
        nameof(CreatedAt),
        nameof(UpdatedAt),
        nameof(Id),
    ]; 

    public string ParseJson()
    {
        var settings = new JsonWriterSettings { OutputMode = JsonOutputMode.RelaxedExtendedJson };
        return this.ToJson(settings);
    }

    #region Init
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = default!;

    [BsonElement(nameof(Queue))]
    public string Queue { get; init; }

    [BsonElement(nameof(CreatedAt))]
    public DateTime CreatedAt { get; init; }

    [BsonElement(nameof(CorrelationId))]
    public string CorrelationId { get; init; } = default!;

    [BsonElement(nameof(PolicyVersion))]
    public int PolicyVersion { get; init; }
    #endregion

    [BsonElement(nameof(State))]
    [BsonRepresentation(BsonType.String)]
    public MessageState State { get; set; }

    [BsonElement(nameof(RetryCount))]
    public int RetryCount { get; private set; } = 0;

    [BsonElement(nameof(LastError))]
    public MessageError? LastError { get; private set; } = null;

    [BsonElement(nameof(Payload))]
    public BsonDocument Payload { get; private set; } = default!;

    [BsonElement(nameof(NextPublishAt))]
    public DateTime? NextPublishAt { get; private set; } = null;

    [BsonElement(nameof(UpdatedAt))]
    public DateTime UpdatedAt { get; private set; } = DateTime.Now;
}
