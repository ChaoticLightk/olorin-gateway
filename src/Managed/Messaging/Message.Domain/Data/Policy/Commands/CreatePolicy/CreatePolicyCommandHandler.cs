using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Data.Policy.Persistence.Abstractions;
using Messaging.Domain.Entities.Queue.Mongo.Policy;
using Microsoft.Extensions.Logging;

namespace Messaging.Domain.Data.Policy.Commands.CreatePolicy;

public class CreatePolicyCommandHandler(
    IQueuePolicyRepository repository,
    ILogger<CreatePolicyCommandHandler> logger) : IHandler<CreatePolicyCommand, bool>
{
    public async Task<bool> Handle(CreatePolicyCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var queueDocument = QueuePolicyDocument.CreateInstance(request.PolicyName);
            await repository.CreatePolicy(queueDocument, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("{message}", ex.Message);
            return false;
        }
    }
}
