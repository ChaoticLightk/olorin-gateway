using System.Collections.ObjectModel;
using Gateway.Domain.Entities.MongoDb.Cluster;
using Gateway.Domain.Entities.MongoDb.Route;
using Gateway.Domain.Options;
using Gateway.Domain.Repositories.Config.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Infrastructure.Repositories.Config;

public class OlorinConfigRepository(
    IMongoClient mongo,
    IOptions<MongoDbOptions> options
) : IConfigRepository
{
    private const string ROUTES_COLLECTION_NAME = "Routes";
    private const string CLUSTERS_COLLECITON_NAME = "Clusters";

    private readonly IMongoCollection<RouteDocument> _routesCollection = mongo 
        .GetDatabase(options.Value.DatabaseName)
        .GetCollection<RouteDocument>(ROUTES_COLLECTION_NAME);

    private readonly IMongoCollection<ClusterDocument> _clustersCollection = mongo 
        .GetDatabase(options.Value.DatabaseName)
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
        var docs = _routesCollection.Find(FilterDefinition<RouteDocument>.Empty).ToList();
        return [.. docs.Select(r => new RouteConfig
        {
            RouteId = r.RouteId,
            ClusterId = r.ClusterId,
            Match = new RouteMatch { Path = r.Match.Path },
            Transforms = r.Transforms
        })];
    }
}
