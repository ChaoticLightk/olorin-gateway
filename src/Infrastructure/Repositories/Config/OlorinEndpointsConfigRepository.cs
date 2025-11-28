using Domain.Entities.Mongo.Endpoint;
using Domain.Repositories.Interfaces;
using Domain.Shared.Constants;
using MongoDB.Driver;
using Yarp.ReverseProxy.Configuration;

namespace Infrastructure.Repositories.Config;

public class OlorinEndpointsConfigRepository(IMongoClient mongo)
    : IConfigRepository
{
    private const string ENDPOINTS_COLLECITON_NAME = "endpoints";

    private readonly IMongoCollection<EndpointDocument> _endpointsCollection = mongo
        .GetDatabase(MongoDbConfiguration.PROXY_DB)
        .GetCollection<EndpointDocument>(ENDPOINTS_COLLECITON_NAME);

    public List<ClusterConfig> GetClusters()
    {
        var docs = _endpointsCollection
            .Find(FilterDefinition<EndpointDocument>.Empty)
            .ToList();

        return [.. docs.Select(c => new ClusterConfig
        {
            ClusterId = c.Cluster.ClusterId,
            LoadBalancingPolicy = c.Cluster.LoadBalancingPolicy,
            Destinations = c.Cluster.Destinations.ToDictionary(
                x => x.Name,
                x => new DestinationConfig {
                    Address = x.Address,
                    Health = x.Health,
                    Metadata = new Dictionary<string, string>()
                    {
                        { "Enabled", x.Enabled.ToString() },
                        { "Weight", x.Weight.ToString() },
                    }
                }
            ),
        })];
    }

    public List<RouteConfig> GetRoutes()
    {
        var docs = _endpointsCollection
            .Find(FilterDefinition<EndpointDocument>.Empty)
            .ToList();

        var routes = new List<RouteConfig>();

        foreach (var endpoint in docs)
        {
            var clusterId = endpoint.Cluster.ClusterId;

            foreach (var route in endpoint.Routes)
            {
                var item = new RouteConfig()
                {
                    RouteId = route.Id,
                    ClusterId = clusterId,
                    Match = new RouteMatch
                    {
                        Path = route.WildcardPath
                    },
                    Metadata = route.BuildMetadata(),
                    Transforms = route.BuildTransforms(),
                    AuthorizationPolicy = route.Authorization
                };

                routes.Add(item);
            }
        }

        return routes;
    }
}
