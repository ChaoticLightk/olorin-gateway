using Package.ResultPattern;
using Package.ResultPattern.ErrorResult;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Data.Policy.Persistence.Abstractions;
using Messaging.Domain.Shared.Messaging.DTO;
using Messaging.Domain.Shared.Messaging.Mappers;
using Messaging.Domain.Shared.Messaging.Services;

namespace Messaging.Domain.Data.Policy.Commands.Create;

public class CreatePolicyCommandHandler(IQueuePolicyRepository repository, IProvisionService queue)
    : IHandler<CreatePolicyCommand, Result>
{
    public async Task<Result> Handle(CreatePolicyCommand request, CancellationToken cancellationToken = default)
    {
        var document = request.MapDocument();

        var provision = ProvisionQueueMapper.MapFromDocument(document);

        try
        {
            await queue.ProvisionAsync(provision, cancellationToken);
        }
        catch (Exception ex)
        {
            return Error.InternalServer($"Não foi possivel declarar a fila {document.QueueName}: {ex.Message}", ex: ex);
        }

        return await repository.CreatePolicy(document, cancellationToken);
    }
}
