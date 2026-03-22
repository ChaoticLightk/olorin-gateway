using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo;

public class MessageError
{
    [BsonElement(nameof(Code))]
    public string Code { get; set; } = default!;

    [BsonElement(nameof(Message))]
    public string Message { get; set; } = default!;

    [BsonElement(nameof(At))]
    public DateTime At { get; set; }
}
