using Gateway.Domain.Entities.MongoDb.Cluster;
using Gateway.Domain.Entities.MongoDb.Route;
using Gateway.Domain.Repositories.Config.Interfaces;
using Gateway.Shared.Constants.Database;
using MongoDB.Driver;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Infrastructure.Repositories.Config;

public class OlorinConfigRepository(IMongoClient mongo) 
    : IConfigRepository
{
    private const string ROUTES_COLLECTION_NAME = "routes";
    private const string CLUSTERS_COLLECITON_NAME = "clusters";

    private readonly IMongoCollection<RouteDocument> _routesCollection = mongo 
        .GetDatabase(MongoDbConfiguration.DB_NAME)
        .GetCollection<RouteDocument>(ROUTES_COLLECTION_NAME);

    private readonly IMongoCollection<ClusterDocument> _clustersCollection = mongo 
        .GetDatabase(MongoDbConfiguration.DB_NAME)
        .GetCollection<ClusterDocument>(CLUSTERS_COLLECITON_NAME);

    public List<ClusterConfig> GetClusters()
    {
        var docs = _clustersCollection
            .Find(FilterDefinition<ClusterDocument>.Empty)
            .ToList();

        return [.. docs.Select(c => new ClusterConfig
        {
            ClusterId = c.ClusterId,
            Destinations = c.Destinations.ToDictionary(
                x => x.Name,
                x => new DestinationConfig {
                    Address = x.Address,
                    Health = x.Health
                }
            ),
        })];
    }

    public List<RouteConfig> GetRoutes()
    {
        var routes = _routesCollection
            .Find(FilterDefinition<RouteDocument>.Empty)
            .ToList();

        var clusters = _clustersCollection
            .Find(FilterDefinition<ClusterDocument>.Empty)
            .ToList();

        var q = from route in routes.AsQueryable()
            join cluster in clusters.AsQueryable()
            on route.ClusterId equals cluster.Id
            select new RouteConfig
            {
                RouteId = route.RouteId,
                ClusterId = cluster.ClusterId,
                Transforms = route.TransformsDicitonary(),
                Match = new() { Path = route.Match.Path },
                AuthorizationPolicy = route.Authorization
            };

        return [.. q];
    }
}
