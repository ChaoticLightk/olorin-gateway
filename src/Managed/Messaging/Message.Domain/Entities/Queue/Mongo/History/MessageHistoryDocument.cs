using System;
using Messaging.Domain.Entities.Queue.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Messaging.Domain.Entities.Queue.Mongo.History;

public class MessageHistoryDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string MessageId { get; set; } = default!;

    [BsonRepresentation(BsonType.String)]
    public MessageEventType EventType { get; set; }

    [BsonRepresentation(BsonType.String)]
    public MessageState FromState { get; set; }

    [BsonRepresentation(BsonType.String)]
    public MessageState ToState { get; set; }

    [BsonElement(nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; }
}
