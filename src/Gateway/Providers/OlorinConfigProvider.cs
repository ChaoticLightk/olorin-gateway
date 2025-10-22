using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Providers;

public interface IConfigRepository
{
    List<RouteConfig> GetRoutes();
    List<ClusterConfig> GetClusters();
}

public class OlorinConfigProvider(IConfigRepository repository) 
    : IProxyConfigProvider
{
    private IProxyConfig _config = LoadConfig(repository);

    public IProxyConfig GetConfig() => _config;

    private static OlorinProxyConfig LoadConfig(IConfigRepository repo)
    {
        var routes = repo.GetRoutes();
        var clusters = repo.GetClusters();

        return new OlorinProxyConfig(routes, clusters); 
    }

    public void Reload()
    {
        _config = LoadConfig(repository);
        ((OlorinProxyConfig)_config).SignalChange();
    }
}

public class OlorinProxyConfig(
    List<RouteConfig> routes,
    List<ClusterConfig> clusters) : IProxyConfig
{
    private readonly List<RouteConfig> _routes = routes;
    private readonly List<ClusterConfig> _clusters = clusters;
    private readonly CancellationTokenSource _cts = new();

    public IReadOnlyList<RouteConfig> Routes => _routes;
    public IReadOnlyList<ClusterConfig> Clusters => _clusters;

    IChangeToken IProxyConfig.ChangeToken => new CancellationChangeToken(_cts.Token);

    public void SignalChange()
    {
        _cts.Cancel();
    }
}
