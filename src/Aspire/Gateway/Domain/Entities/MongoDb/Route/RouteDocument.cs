using Gateway.Domain.Entities.MongoDb.Cluster;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.MongoDb.Route;

public class RouteDocument
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    [BsonElement("routeId")]
    public string RouteId { get; set; } = default!;

    [BsonElement("cluster")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ClusterId { get; set; } = default!;
    
    [BsonElement("match")]
    public MatchDocument Match { get; set; } = new();

    [BsonElement("transforms")]
    public List<Dictionary<string, string>>? Transforms { get; set; }

    [BsonElement("__v")]
    public int Test { get; set; }
}
