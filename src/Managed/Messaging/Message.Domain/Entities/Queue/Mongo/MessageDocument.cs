using Messaging.Domain.Entities.Queue.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo;

public class MessageDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = default!;

    [BsonElement(nameof(Queue))]
    public string Queue { get; set; } = default!;

    [BsonElement(nameof(PolicyVersion))]
    public int PolicyVersion { get; set; }

    [BsonElement(nameof(State))]
    [BsonRepresentation(BsonType.String)]
    public MessageState State { get; set; }

    [BsonElement(nameof(RetryCount))]
    public int RetryCount { get; set; }

    [BsonElement(nameof(LastError))]
    public MessageError? LastError { get; set; } = null;

    [BsonElement(nameof(CorrelationId))]
    public string CorrelationId { get; set; } = default!;

    [BsonElement(nameof(Payload))]
    public BsonDocument Payload { get; set; } = default!;

    [BsonElement(nameof(NextPublishAt))]
    public DateTime? NextPublishAt { get; set; } = null;

    [BsonElement(nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; }

    [BsonElement(nameof(UpdatedAt))]
    public DateTime UpdatedAt { get; set; }
}
