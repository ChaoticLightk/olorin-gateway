using Package.ResultPattern;
using Package.ResultPattern.ErrorResult;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Data.Policy.Persistence.Abstractions;
using Messaging.Domain.Shared.Messaging.Mappers;
using Messaging.Domain.Shared.Messaging.Services;

namespace Messaging.Domain.Data.Policy.Commands.Provision;

public class ProvisionPolicyCommandHandler(
    IQueuePolicyRepository repository,
    IProvisionService service) 
    : IHandler<ProvisionPolicyCommand, Result>
{
    public async Task<Result> Handle(
        ProvisionPolicyCommand request,
        CancellationToken cancellationToken = default)
    {
        var result = await repository
            .Get(request.QueueName, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var document = result.Value; 

        if (document is null)
        {
            return Error.NotFound($"Política não encontrada para a fila {request.QueueName}");
        }

        var provision = ProvisionQueueMapper.MapFromDocument(document);

        try
        {
            await service.ProvisionAsync(provision, cancellationToken);
        }
        catch(Exception ex)
        {
            return Error.InternalServer("Não foi possível provisionar a fila " + ex.Message);
        }

        return Result.Ok();
    }
}
