using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.Mongo.Route;

public class TransformDocument
{
    [BsonElement("type")]
    public string Type { get; set; } = default!;

    [BsonElement("value")]
    public string Value { get; set; } = default!;

    [BsonElement("when")]
    public string When { get; set; } = default!;
}
