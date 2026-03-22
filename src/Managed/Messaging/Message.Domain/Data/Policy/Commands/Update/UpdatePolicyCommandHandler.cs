using Package.ResultPattern;
using Package.ResultPattern.ErrorResult;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Data.Policy.Persistence.Abstractions;

namespace Messaging.Domain.Data.Policy.Commands.Update;

public class UpdatePolicyCommandHandler(IQueuePolicyRepository repository) 
    : IHandler<UpdatePolicyCommand, Result>
{
    public async Task<Result> Handle(UpdatePolicyCommand request, CancellationToken cancellationToken = default)
    {
        var document = request.MapDocument();

        var existingDocumentResult = await repository
            .Get(document.QueueName, cancellationToken);

        if (existingDocumentResult.IsFailure)
        {
            return existingDocumentResult;
        }

        var existingDocument = existingDocumentResult.Value;

        if (existingDocument is null)
        {
            return Error.NotFound($"Não foi encontrado registro para o Id {document.QueueName}"); 
        }

        document.UpgradeVersion();

        return await repository.UpdatePolicy(document, cancellationToken);
    }
}
