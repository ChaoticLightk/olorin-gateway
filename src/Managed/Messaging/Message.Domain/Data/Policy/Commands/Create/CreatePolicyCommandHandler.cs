using Colombo.ResultPattern;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Persistence.Abstractions;

namespace Messaging.Domain.Data.Policy.Commands.Create;

public class CreatePolicyCommandHandler(IQueuePolicyRepository repository) 
    : IHandler<CreatePolicyCommand, Result>
{
    public async Task<Result> Handle(CreatePolicyCommand request, CancellationToken cancellationToken = default)
    {
        var queueDocument = request.MapDocument();
        var result = await repository.CreatePolicy(queueDocument, cancellationToken);
        return result;
    }
}
