using Gateway.Domain.Providers.Interfaces;
using Gateway.Domain.Repositories.Interfaces;
using Gateway.Providers.Config;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Providers;

public class MongoConfigProvider(IConfigRepository repository) 
    : IProxyConfigProvider, IReloadableProxyConfigProvider
{
    private IProxyConfig _config = LoadConfig(repository);

    public IProxyConfig GetConfig() => _config;

    private readonly Lock _lock = new();

    private static MongoProxyConfig LoadConfig(IConfigRepository repo)
    {
        var routes = repo.GetRoutes();
        var clusters = repo.GetClusters();

        return new MongoProxyConfig(routes, clusters); 
    }

    public void Reload()
    {
        lock(_lock)
        {
            var @new = LoadConfig(repository);
            var old = _config;

            _config = @new;
            ((MongoProxyConfig)old).SignalChange();
        }
    }
}

