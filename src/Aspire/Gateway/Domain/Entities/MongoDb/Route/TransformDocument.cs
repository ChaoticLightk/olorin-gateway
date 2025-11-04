using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.MongoDb.Route;

public class TransformDocument
{
    [BsonElement("type")]
    public string Type { get; set; } = default!;

    [BsonElement("value")]
    public string Value { get; set; } = default!;
}
