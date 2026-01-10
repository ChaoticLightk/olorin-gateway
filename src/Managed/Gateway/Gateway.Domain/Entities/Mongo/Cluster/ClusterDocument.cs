using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.Mongo.Cluster;

public class ClusterDocument
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    [BsonElement("clusterId")]
    public string ClusterId { get; set; } = default!;

    [BsonElement("destinations")]
    public List<DestinationDocument> Destinations { get; set; } = [];
}
