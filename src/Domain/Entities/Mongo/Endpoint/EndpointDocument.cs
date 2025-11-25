using System.ComponentModel;
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

        [BsonElement("bodyTransformType")]
        public BodyTransformType BodyTransformType { get; set; }

        [BsonElement("rootTransform")]
        public string BodyTransformRoot { get; set; } = string.Empty; 

        [BsonIgnoreIfNull]
        [BsonElement("transforms")]
        public List<TransformDocument>? Transforms { get; set; } = [];

        public IReadOnlyDictionary<string, string> BuildMetadata()
        {
            var metadata = new Dictionary<string, string>();

            if (BodyTransform)
            {
                metadata[nameof(BodyTransform)] = true.ToString();
                metadata[nameof(BodyTransformType)] = BodyTransformType.ToString();
                metadata[nameof(BodyTransformRoot)] = BodyTransformRoot.ToString();
            }

            return metadata;
        }

        public IReadOnlyList<IReadOnlyDictionary<string, string>> BuildTransforms()
        {
            if (Transforms is null)
            {
                return [];
            }

            return Transforms
                .Select(p => new Dictionary<string, string>
                    {{ p.Type, p.Value } })
                .ToList();
        }

        public bool BodyTransform => BodyTransformType != BodyTransformType.None;
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

    public enum BodyTransformType
    {
        [Description("None")]
        None = 0,

        [Description("Transform a XML response into JSON")]
        XmlToJson = 1
    }
}

