using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo.Policy;

public class RetryPolicy
{
    private RetryPolicy() { }

    public static RetryPolicy CreateInstance()
    {
        return new RetryPolicy();
    }

    [BsonElement(nameof(MaxRetries))]
    public ushort MaxRetries { get; set; } = 0;

    [BsonElement(nameof(BaseDelaySeconds))]
    public uint BaseDelaySeconds { get; set; } = 0;

    [BsonElement(nameof(PenaltyFactor))]
    public double PenaltyFactor { get; set; } = 0;

    [BsonElement(nameof(MaxDelaySeconds))]
    public uint MaxDelaySeconds { get; set; } = 0;
}