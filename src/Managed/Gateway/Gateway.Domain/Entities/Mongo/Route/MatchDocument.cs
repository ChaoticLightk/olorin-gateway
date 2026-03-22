using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.Mongo.Route;

public class MatchDocument
{
    [BsonElement("path")]
    public string? Path { get; set; }
}
