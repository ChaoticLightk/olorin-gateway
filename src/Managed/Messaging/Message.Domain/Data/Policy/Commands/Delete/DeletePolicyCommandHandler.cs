using Package.ResultPattern;
using Package.ResultPattern.ErrorResult;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Data.Policy.Persistence.Abstractions;

namespace Messaging.Domain.Data.Policy.Commands.Delete;

public class DeletePolicyCommandHandler(IQueuePolicyRepository repository) 
    : IHandler<DeletePolicyCommand, Result>
{
    public async Task<Result> Handle(DeletePolicyCommand request, CancellationToken cancellationToken = default)
    {
        var result = await repository.Get(
            request.QueueName,
            cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var value = result.Value;

        if (value is null)
        {
            return Error.NotFound($"Não foi encontrado registro para o Id {request.QueueName}"); 
        }

        value.SoftDelete();

        var updateResult = await repository.UpdatePolicy(value, cancellationToken); 

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }
    
        return Result.Ok();
    }
}
