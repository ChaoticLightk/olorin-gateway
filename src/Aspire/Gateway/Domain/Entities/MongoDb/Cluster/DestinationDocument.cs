using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.MongoDb.Cluster;

public class DestinationDocument
{

    [BsonElement("name")]
    public string Name { get; set; } = default!;

    [BsonElement("address")]
    public string Address { get; set; } = default!;

    [BsonElement("health")]
    public string Health { get; set; } = default!;
}
