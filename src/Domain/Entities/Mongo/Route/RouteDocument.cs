using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.Mongo.Route;

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
    public List<TransformDocument>? Transforms { get; set; } = [];

    [BsonElement("authorizationPolicy")]
    public string AuthorizationPolicy { get; set; } = default!;

    [BsonElement("authorization")]
    public string Authorization { get; set; } = "Anonymous";

    public IReadOnlyList<IReadOnlyDictionary<string, string>> TransformsDicitonary()
    {
        if (Transforms is null)
        {
            return [];
        }

        return Transforms
            .Select(p => new Dictionary<string, string>
                {{ p.Type, p.Value } })
            .ToList();
    }
}
