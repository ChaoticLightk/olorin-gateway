using Colombo.ResultPattern;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Persistence.Abstractions;
using Messaging.Domain.Shared.Messaging.Services;

namespace Messaging.Domain.Data.Policy.Commands.Create;

public class CreatePolicyCommandHandler(IQueuePolicyRepository repository, IQueueService queue) 
    : IHandler<CreatePolicyCommand, Result>
{
    public async Task<Result> Handle(CreatePolicyCommand request, CancellationToken cancellationToken = default)
    {
        var queueDocument = request.MapDocument();

        await queue.ProvisionAsync(queueDocument, cancellationToken); 

        return await repository.CreatePolicy(queueDocument, cancellationToken);
    }
}
