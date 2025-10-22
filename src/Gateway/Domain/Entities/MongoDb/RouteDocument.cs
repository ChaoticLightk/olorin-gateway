using MongoDB.Bson.Serialization.Attributes;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Domain.Entities.MongoDb;

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
    public RouteMatch Match { get; set; } = new();

    [BsonElement("transforms")]
    public List<Dictionary<string, string>>? Transforms { get; set; }
}
