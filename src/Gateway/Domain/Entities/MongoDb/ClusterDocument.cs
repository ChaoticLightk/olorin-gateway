using MongoDB.Bson.Serialization.Attributes;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Domain.Entities.MongoDb;

public class ClusterDocument
{
    [BsonId]
    public string ClusterId { get; set; } = default!;

    public Dictionary<string, DestinationConfig> Destinations { get; set; } = new();
}
