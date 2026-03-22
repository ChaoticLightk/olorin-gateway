using Messaging.Domain.Data.Policy.Enums;

namespace Messaging.Domain.Data.Policy.Queries.Get;

public record GetPoliciesQuery(int Page, int PageSize, FilterQueuePolicy Filter = FilterQueuePolicy.None);
