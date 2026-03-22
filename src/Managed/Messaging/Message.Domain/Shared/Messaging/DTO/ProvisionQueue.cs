namespace Messaging.Domain.Shared.Messaging.DTO;

public class RabbitMqQueueType
{
    private RabbitMqQueueType(string @type)
    {
        Type = @type;
    }

    public string Type { get; init; }

    public static RabbitMqQueueType Quorum => new("quorum");
    public static RabbitMqQueueType Classic => new("classic");
}

public enum QueueType
{
    Classic,
    Quorum,
    Stream
}

public record ProvisionQueueRetryPolicy(
    bool EnableRetry,
    uint MaxRetryCount
);

public record ProvisionQueue(
    string RoutingKey,
    bool Enabled,
    RabbitMqQueueType QueueType,
    ProvisionQueueRetryPolicy? RetryPolicy = null,
    bool EnabledDeadLetter = false
)
{
    public static ProvisionQueue Classic(
        string routingKey,
        bool enabled = true,
        bool enabledDeadLetter = false
    )
    {
        return new(
            routingKey,
            enabled,
            RabbitMqQueueType.Classic,
            EnabledDeadLetter: enabledDeadLetter);
    }

    // TODO: check instance diffs between stream and quorum
    public static ProvisionQueue Stream(
        string routingKey,
        bool enabled = true,
        bool enabledDeadLetter = false
    )
    {
        return new(
            routingKey,
            enabled,
            RabbitMqQueueType.Classic,
            EnabledDeadLetter: enabledDeadLetter);
    }

    public static ProvisionQueue Quorum(
        string routingKey,
        bool enableRetry = false,
        uint deliveryLimit = 0,
        bool enabled = true,
        bool enabledDeadLetter = false
    )
    {
        var retryPolicy = new ProvisionQueueRetryPolicy(
            enableRetry,
            deliveryLimit);

        return new(
            routingKey,
            enabled,
            RabbitMqQueueType.Quorum,
            retryPolicy,
            enabledDeadLetter);
    }
}