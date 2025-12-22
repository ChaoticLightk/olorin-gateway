using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo.Policy;

public class QueuePolicyDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string QueueName { get; set; } = default!;

    [BsonElement(nameof(Enabled))]
    public bool Enabled { get; set; }

    [BsonElement(nameof(EnabledDeadLetter))]
    public bool EnabledDeadLetter { get; set; } = false;

    [BsonElement(nameof(RetryPolicy))]
    public RetryPolicy RetryPolicy { get; set; } = new();

    [BsonElement(nameof(Version))]
    public short Version { get; set; }

    [BsonElement(nameof(Metadata))]
    public QueueMetadata Metadata { get; set; } = new();

    [BsonElement(nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; }

    [BsonElement(nameof(UpdatedAt))]
    public DateTime UpdatedAt { get; set; }

    [BsonElement(nameof(DeletedAt))]
    public DateTime? DeletedAt { get; set; } = null;
}