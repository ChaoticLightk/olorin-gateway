using Gateway.Domain.Repositories.Config.Interfaces;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Infrastructure.Repositories.Config;

public class OlorinConfigRepository : IConfigRepository
{
    public List<ClusterConfig> GetClusters()
    {
        return [
            new() {
                ClusterId = "apiCluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "pessimistic", new DestinationConfig { Address = "https://localhost:7200/" } },
                    { "optmistic", new DestinationConfig { Address = "https://localhost:7100/" } }
                }
            }
        ];
    }

    public List<RouteConfig> GetRoutes()
    {
        return [
            new(){
                RouteId = "weatherRoute",
                ClusterId = "apiCluster",
                Match = new()  {
                    Path = "/weatherforecast"
                }
            }
        ];
    }
}
