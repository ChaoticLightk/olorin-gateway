using Colombo.ResultPattern;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Data.Policy.Queries.DTO;
using Messaging.Domain.Persistence.Abstractions;

namespace Messaging.Domain.Data.Policy.Queries.Get;

public class GetPoliciesQueryHandler(IQueuePolicyRepository repository) 
    : IHandler<GetPoliciesQuery, Result<List<PolicyResponse>>>
{
    public async Task<Result<List<PolicyResponse>>> Handle(
        GetPoliciesQuery request,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.Get(cancellationToken);

        if (result.IsFailure)
        {
            return result.Error!; 
        }

        var value = result.Value;

        if (value is null or [])
        {
            return Result<List<PolicyResponse>>.Success([]); 
        }

        return value.Select(PolicyResponse.FromDocument).ToList();
    }
}
