using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

namespace Gateway.Extensions.YARP.LoadBalancing;

public class WeightedLoadBalancingPolicy : ILoadBalancingPolicy
{
    public string Name => "Weighted";

    public DestinationState? PickDestination(
        HttpContext context,
        ClusterState cluster,
        IReadOnlyList<DestinationState> availableDestinations)
    {
        if (!availableDestinations.Any())
        {
            return null;
        }

        var WeightedList = availableDestinations
            .Select(dest =>
            {
                if (dest.Model.Config.Metadata?.TryGetValue("Weight", out var weightStr) == true
                    && int.TryParse(weightStr, out var weight)
                    && weight > 0)
                {
                    return (dest, weight);
                }

                return (dest, 0);
            });

        return FallBack(availableDestinations);
    }

    private static DestinationState FallBack(IReadOnlyList<DestinationState> availableDestinations)
    {
        return availableDestinations[^1];
    }
}
