using System.Dynamic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo.Policy;

public class QueueMetadata
{
    private QueueMetadata()
    {
    }

    public static QueueMetadata CreateInstance(string description, string owner)
    {
        return new QueueMetadata()
        {
            Description = description,
            Owner = owner
        };
    } 

    [BsonElement(nameof(Description))]
    public string? Description { get; set; }

    [BsonElement(nameof(Owner))]
    public string? Owner { get; set; }
}