using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.MongoDb.Route;

public class RouteDocument
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string Id { get; set; } = default!;

    [BsonElement("routeId")]
    public string RouteId { get; set; } = default!;

    [BsonElement("clusterId")]
    public string ClusterId { get; set; } = default!;

    [BsonElement("match")]
    public MatchDocument Match { get; set; } = new();

    [BsonElement("transforms")]
    public List<Dictionary<string, string>>? Transforms { get; set; }
}
