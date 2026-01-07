using Colombo.ResultPattern;
using Messaging.Domain.Entities.Queue.Mongo;

namespace Messaging.Domain.Data.Messaging.Persistence.Abstractions;

public interface IQueueMessageRepository
{
    Task<Result<MessageDocument>> CreateMessage(MessageDocument document, CancellationToken cancellationToken = default);

    Task<Result> UpdateMessage(MessageDocument document, CancellationToken cancellationToken = default);
}
