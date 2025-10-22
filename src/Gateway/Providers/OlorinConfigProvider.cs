using Gateway.Domain.Repositories.Config.Interfaces;
using Gateway.Providers.Config;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Providers;

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

