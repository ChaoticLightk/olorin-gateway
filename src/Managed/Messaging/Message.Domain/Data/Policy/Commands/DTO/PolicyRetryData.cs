namespace Messaging.Domain.Data.Policy.Commands.DTO;

public record PolicyRetryData(
    ushort MaxRetries,
    uint DelaySeconds,
    double PenalyFactor,
    uint MaxDelaySeconds
);