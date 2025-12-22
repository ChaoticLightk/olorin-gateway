namespace Messaging.Domain.Entities.Queue.Enums;

public enum MessageState
{
    Created,
    Published,
    Processing,
    Failed,
    Scheduled,
    Completed,
    DeadLetter,
    Paused,
    Cancelled
}