using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo.Policy;

public class QueueMetadata
{
    [BsonElement(nameof(Description))]
    public string? Description { get; set; }

    [BsonElement(nameof(Owner))]
    public string? Owner { get; set; }
}