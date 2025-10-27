using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.MongoDb.Cluster;

public class ClusterDocument
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    [BsonElement("clusterId")]
    public string ClusterId { get; set; } = default!;

    [BsonElement("destinations")]
    public List<DestinationDocument> Destinations { get; set; } = [];
}
