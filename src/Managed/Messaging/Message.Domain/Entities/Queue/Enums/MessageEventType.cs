namespace Messaging.Domain.Entities.Queue.Enums;

public enum MessageEventType
{
    Created,
    Published,
    ProcessingStarted,
    ProcessingFailed,
    RetryScheduled,
    ManualReprocessRequested,
    Completed,
    DeadLettered,
    Cancelled,
    QueuePaused,
    QueueResumed
}

