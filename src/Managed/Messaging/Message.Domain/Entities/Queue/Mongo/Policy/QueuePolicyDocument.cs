using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo.Policy;

public class QueuePolicyDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string QueueName { get; set; }

    [BsonElement(nameof(Enabled))]
    public bool Enabled { get; set; } = false;

    [BsonElement(nameof(EnabledDeadLetter))]
    public bool EnabledDeadLetter { get; set; } = false;

    [BsonElement(nameof(Version))]
    public int Version { get; set; } = 1;

    [BsonElement(nameof(RetryPolicy))]
    public RetryPolicy? RetryPolicy { get; set; } 

    [BsonElement(nameof(Metadata))]
    public QueueMetadata? Metadata { get; set; }

    [BsonElement(nameof(CreatedAt))]
    public DateTime CreatedAt { get; private set; }

    [BsonElement(nameof(UpdatedAt))]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement(nameof(DeletedAt))]
    public DateTime? DeletedAt { get; set; } = null;

    private QueuePolicyDocument(string name)
    {
       QueueName = name; 
       CreatedAt = DateTime.Now;
    }

    public static QueuePolicyDocument CreateInstance(string name)
        => new(name);

    public static QueuePolicyDocument CreateInstance(
        string name,
        QueueMetadata? metadata
    )
    {
        return new QueuePolicyDocument(name)
        {
            Metadata = metadata
        };
    }

    public static QueuePolicyDocument CreateInstance(
        string name,
        RetryPolicy? retry
    )
    {
        return new QueuePolicyDocument(name)
        {
            RetryPolicy = retry,
        };
    }

    public static QueuePolicyDocument CreateInstance(
        string name,
        RetryPolicy? retry,
        QueueMetadata? metadata,
        bool enabled = false,
        bool enabledDeadLetter = false
    )
    {
        return new QueuePolicyDocument(name)
        {
            RetryPolicy = retry,
            Metadata = metadata,
            Enabled = enabled,
            EnabledDeadLetter = enabledDeadLetter 
        };
    }

    public void UpgradeVersion()
    {
        Version++;
        UpdatedAt = DateTime.Now;
    }

    public void SoftDelete()
    {
        Enabled = false;
        EnabledDeadLetter = false;
        DeletedAt = DateTime.Now;
    }
}