using Messaging.Domain.Data.Policy.Commands.DTO;
using Messaging.Domain.Entities.Queue.Mongo.Policy;

namespace Messaging.Domain.Data.Policy.Commands.Update;

public record UpdatePolicyCommand(PolicyQueueData Policy)
{
    public QueuePolicyDocument MapDocument()
        => Policy.MapDocument();
};