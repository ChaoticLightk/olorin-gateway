using Colombo.ResultPattern;
using Colombo.ResultPattern.ErrorResult;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Data.Policy.Persistence.Abstractions;
using Messaging.Domain.Shared.Messaging.Services;

namespace Messaging.Domain.Data.Policy.Commands.Provision;

public class ProvisionPolicyCommandHandler(
    IQueuePolicyRepository repository,
    IQueueService queue) 
    : IHandler<ProvisionPolicyCommand, Result>
{
    public async Task<Result> Handle(
        ProvisionPolicyCommand request,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.Get(request.QueueName, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var policy = result.Value; 

        if (policy is null)
        {
            return Error.NotFound($"Política não encontrada para a fila {request.QueueName}");
        }

        try
        {
            await queue.ProvisionAsync(policy, cancellationToken);
        }
        catch(Exception ex)
        {
            return Error.InternalServer("Não foi possível provisionar a fila" + ex.Message);
        }

        return Result.Success();
    }
}
