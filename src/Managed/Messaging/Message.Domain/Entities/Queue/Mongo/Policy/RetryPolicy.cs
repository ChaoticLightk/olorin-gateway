using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo.Policy;

public class RetryPolicy
{
    [BsonElement(nameof(MaxRetries))]
    public int MaxRetries { get; set; }

    [BsonElement(nameof(BaseDelaySeconds))]
    public int BaseDelaySeconds { get; set; }

    [BsonElement(nameof(PenaltyFactor))]
    public double PenaltyFactor { get; set; }

    [BsonElement(nameof(MaxDelaySeconds))]
    public int MaxDelaySeconds { get; set; }
}