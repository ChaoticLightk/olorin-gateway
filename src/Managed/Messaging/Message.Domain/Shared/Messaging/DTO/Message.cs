using System.Text.Json;
using Messaging.Domain.Entities.Queue.Enums;

namespace Messaging.Domain.Shared.Messaging.DTO;

public record LastError(
    string Code,
    string Message,
    DateTime At
);

public record Message(
    string Queue,
    int PolicyVersion,
    MessageState State,
    int RetryCount,
    JsonElement Payload,
    LastError? LastError = null,
    DateTime? NextPublishAt = null
)
{
    public static Message CreateInstance(
        string queue,
        int policyVersion,
        MessageState state,
        int retryCount,
        JsonElement payload,
        LastError? lastError = null,
        DateTime? nextPublishAt = null
    )
    {
        return new(
            queue,
            policyVersion,
            state,
            retryCount,
            payload,
            lastError,
            nextPublishAt);
    }
}
