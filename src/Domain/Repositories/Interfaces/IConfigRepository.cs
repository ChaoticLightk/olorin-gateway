using Yarp.ReverseProxy.Configuration;

namespace Domain.Repositories.Interfaces;

public interface IConfigRepository
{
    List<RouteConfig> GetRoutes();
    List<ClusterConfig> GetClusters();
}
