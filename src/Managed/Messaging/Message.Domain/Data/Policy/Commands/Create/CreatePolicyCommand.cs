using Messaging.Domain.Data.Policy.Commands.DTO;
using Messaging.Domain.Entities.Queue.Mongo.Policy;

namespace Messaging.Domain.Data.Policy.Commands.Create;

public record CreatePolicyCommand(PolicyQueueData Policy)
{
    public QueuePolicyDocument MapDocument()
        => Policy.MapDocument();
}
