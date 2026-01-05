namespace Messaging.Domain.Data.Messaging.Commands.Send;

public record SendMessageCommand(string QueueName, string CorrelationId, object Content);
