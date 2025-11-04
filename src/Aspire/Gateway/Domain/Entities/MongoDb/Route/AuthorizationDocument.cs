using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gateway.Domain.Entities.MongoDb.Route;

public class AuthorizationDocument
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    [BsonElement("scheme")]
    public string Scheme { get; set; } = default!;
}
