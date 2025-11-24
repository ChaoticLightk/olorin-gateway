using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

namespace Gateway.Extensions.YARP.LoadBalancing;

public class EnabledAwareLoadBalancingPolicy : ILoadBalancingPolicy
{
    public string Name => "EnableAware";

    private static readonly Random _random = new();

    public DestinationState? PickDestination(
        HttpContext context,
        ClusterState cluster,
        IReadOnlyList<DestinationState> availableDestinations)
    {
        var enabled = availableDestinations
            .Where(x =>
            {
                if (x.Model.Config.Metadata?.TryGetValue("Enabled", out var enabledStr) == true)
                {
                    return string.Equals(enabledStr, "true", StringComparison.OrdinalIgnoreCase);
                }

                return true;
            }).ToList();

        if (enabled.Count > decimal.Zero)
        {
            return enabled[_random.Next(enabled.Count)]; 
        }

        return FallBack(availableDestinations);
    }

    private static DestinationState FallBack(IReadOnlyList<DestinationState> availableDestinations)
        => availableDestinations[
            _random.Next(availableDestinations.Count)];
}
