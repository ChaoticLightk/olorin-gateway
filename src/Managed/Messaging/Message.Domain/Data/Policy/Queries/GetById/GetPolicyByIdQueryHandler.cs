
using Colombo.ResultPattern;
using Colombo.ResultPattern.ErrorResult;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Data.Policy.Queries.DTO;
using Messaging.Domain.Persistence.Abstractions;

namespace Messaging.Domain.Data.Policy.Queries.GetById;

public class GetPolicyByIdQueryHandler(IQueuePolicyRepository repository) 
    : IHandler<GetPolicyByIdQuery, Result<PolicyResponse>>
{
    public async Task<Result<PolicyResponse>> Handle(
        GetPolicyByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.Get(request.QueueName, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error!;
        }

        var value = result.Value;

        if (value is null)
        {
            return Error.NotFound($"Não foi possivel encontrar um politica para o Id {request.QueueName}");
        }

        return PolicyResponse.FromDocument(value);
    }
}
