using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.Mongo.Cluster;

public class DestinationDocument
{
    [BsonElement("name")]
    public string Name { get; set; } = default!;

    [BsonElement("address")]
    public string Address { get; set; } = default!;

    [BsonElement("health")]
    public string Health { get; set; } = default!;
}
