using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Providers.Config;

public class MongoProxyConfig(
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
