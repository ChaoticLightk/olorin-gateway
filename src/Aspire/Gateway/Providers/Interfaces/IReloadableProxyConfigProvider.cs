using System;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Providers.Interfaces;

public interface IReloadableProxyConfigProvider
{
    void Reload();
}
