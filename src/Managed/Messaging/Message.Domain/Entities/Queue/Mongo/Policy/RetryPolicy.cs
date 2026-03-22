using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo.Policy;

public class RetryPolicy
{
    private RetryPolicy() { }

    public static RetryPolicy CreateInstance(
        ushort maxRetries,
        uint delayInSeconds,
        double penaltyFactor,
        uint maxDelayInSeconds
    )
    {
        return new RetryPolicy()
        {
            MaxRetries = maxRetries,
            BaseDelaySeconds = delayInSeconds,
            PenaltyFactor = penaltyFactor,
            MaxDelaySeconds = maxDelayInSeconds
        };
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