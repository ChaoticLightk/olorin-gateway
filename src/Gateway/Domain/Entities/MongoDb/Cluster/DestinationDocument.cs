using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.MongoDb.Cluster;

public class DestinationDocument
{
    [BsonElement("address")]
    public string Address { get; set; } = default!;
}
