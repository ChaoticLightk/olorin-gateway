using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

namespace Gateway.Extensions.YARP.LoadBalancing;

public class WeightedLoadBalancingPolicy : ILoadBalancingPolicy
{
    public string Name => "Weighted";
    private static readonly Random _random = new();

    public DestinationState? PickDestination(
        HttpContext context,
        ClusterState cluster,
        IReadOnlyList<DestinationState> availableDestinations)
    {
        if (!availableDestinations.Any())
        {
            return null;
        }

        var weightedList = availableDestinations
            .Select(dest =>
            {
                if (dest.Model.Config.Metadata?.TryGetValue("Weight", out var weightStr) == true
                    && int.TryParse(weightStr, out var weight)
                    && weight > 0)
                {
                    return (dest, weight);
                }

                return (dest, weight: 0);
            });

        var totalWeight = weightedList.Sum(x => x.weight);

        if (totalWeight == 0)
        {
            return FallBack(availableDestinations);
        }

        int randomValue = _random.Next(1, totalWeight + 1);

        int cumulative = 0;

        foreach (var (destination, weight) in weightedList)
        {
            cumulative += weight;

            if (randomValue <= cumulative)
            {
                return destination;
            }
        }

        return FallBack(availableDestinations);
    }

    private static DestinationState FallBack(IReadOnlyList<DestinationState> availableDestinations)
        => availableDestinations[
            _random.Next(availableDestinations.Count)];
}
