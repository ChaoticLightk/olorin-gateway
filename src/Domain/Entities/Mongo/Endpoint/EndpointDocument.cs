using Domain.Entities.Mongo.Route;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.Mongo.Endpoint
{
    public class RouteDocument
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;

        [BsonElement("authorization")]
        public string Authorization { get; set; } = "Anonymous";

        [BsonElement("match")]
        public MatchDocument Match { get; set; } = default!;

        [BsonElement("transforms")]
        [BsonIgnoreIfNull]
        public List<TransformDocument>? Transforms { get; set; } = [];
    }

    public class DestinationDocument
    {
        [BsonElement("name")]
        public string Name { get; set; } = default!;

        [BsonElement("address")]
        public string Address { get; set; } = default!;

        [BsonElement("health")]
        [BsonIgnoreIfNull]
        public string? Health { get; set; }

        [BsonElement("enabled")]
        public bool Enabled { get; set; } = true;

        [BsonElement("weight")]
        [BsonIgnoreIfNull]
        public int Weight { get; set; } = 0;
    }

    public class ClusterDocument
    {
        [BsonElement("clusterId")]
        public string ClusterId { get; set; } = default!;

        [BsonElement("loadBalancingPolicy")]
        public string LoadBalancingPolicy { get; set; } = default!;

        [BsonElement("destinations")]
        public List<DestinationDocument> Destinations { get; set; } = [];
    }

    public class EndpointDocument
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;

        [BsonElement("cluster")]
        public ClusterDocument Cluster { get; set; } = default!;

        [BsonElement("routes")]
        public List<RouteDocument> Routes { get; set; } = [];

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

