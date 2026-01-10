using Package.ResultPattern;
using Messaging.Domain.Data.Abstractions.Wolwerine;

namespace Messaging.Domain.Data.Messaging.Queries.Get;

public record GetMessagesResponse(
    string QueueName,
    string CorrelationId,
    string State,
    string Payload,
    int RetryCount,
    int PolicyVersion,
    DateTime NextPublishAt,
    DateTime UpdatedAt,
    DateTime CreatedAt
);

public class GetMessagesQueryHandler 
    : IHandler<GetMessagesQuery, Result<List<GetMessagesResponse>>>
{
    public Task<Result<List<GetMessagesResponse>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
