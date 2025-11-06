using Domain.Repositories.Interfaces;
using Gateway.Providers.Config;
using Gateway.Providers.Interfaces;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Providers;

public class OlorinConfigProvider(IConfigRepository repository) 
    : IProxyConfigProvider, IReloadableProxyConfigProvider
{
    private IProxyConfig _config = LoadConfig(repository);

    public IProxyConfig GetConfig() => _config;

    private readonly Lock _lock = new();

    private static OlorinProxyConfig LoadConfig(IConfigRepository repo)
    {
        var routes = repo.GetRoutes();
        var clusters = repo.GetClusters();

        return new OlorinProxyConfig(routes, clusters); 
    }

    public void Reload()
    {
        lock(_lock)
        {
            var @new = LoadConfig(repository);
            var old = _config;

            _config = @new;
            ((OlorinProxyConfig)old).SignalChange();
        }
    }
}

