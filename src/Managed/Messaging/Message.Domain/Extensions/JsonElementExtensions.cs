using System.Text.Json;
using MongoDB.Bson;

namespace Messaging.Domain.Extensions;

public static class JsonElementExtensions
{
    public static BsonDocument ParseToBsonDocument(this JsonElement element)
    {
        return BsonDocument.Parse(element.GetRawText());
    }
}
