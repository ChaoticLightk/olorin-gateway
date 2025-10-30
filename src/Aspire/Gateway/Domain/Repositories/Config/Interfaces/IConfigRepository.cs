using Yarp.ReverseProxy.Configuration;

namespace Gateway.Domain.Repositories.Config.Interfaces;

public interface IConfigRepository
{
    List<RouteConfig> GetRoutes();
    List<ClusterConfig> GetClusters();
}
