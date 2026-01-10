using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.Mongo.Application;

public class ApplicationDocument
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    [BsonElement("name")]
    public string Name { get; set; } = default!;

    [BsonElement("password")]
    public string Password { get; set; } = default!;

    [BsonElement("salt")]
    public string Salt { get; set; } = default!;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}
