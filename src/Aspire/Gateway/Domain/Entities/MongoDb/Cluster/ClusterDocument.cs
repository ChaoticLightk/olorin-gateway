using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.MongoDb.Cluster;

public class ClusterDocument
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string Id { get; set; } = default!;

    [BsonElement("clusterId")]
    public string ClusterId { get; set; } = default!;

    [BsonElement("destinations")]
    public Dictionary<string, DestinationDocument> Destinations { get; set; } = [];
}
