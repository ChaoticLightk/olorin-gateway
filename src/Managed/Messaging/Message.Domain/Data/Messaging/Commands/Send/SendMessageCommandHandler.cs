using Colombo.ResultPattern;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Entities.Queue.Mongo;
using Messaging.Domain.Persistence.Abstractions;
using Messaging.Domain.Shared.Messaging.Services;

namespace Messaging.Domain.Data.Messaging.Commands.Send;

public class SendMessageCommandHandler(
    IQueueService queue,
    IQueuePolicyRepository policyRepository): IHandler<SendMessageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        SendMessageCommand request, 
        CancellationToken cancellationToken = default)
    {
        var policyResult = await policyRepository
            .Get(request.QueueName, cancellationToken);

        if (policyResult.IsFailure)
        {
            return policyResult.Error!;
        }

        var policy = policyResult.Value;

        if (policy is null)
        {
            // TODO: log, policy notfound, following with default policy
        }

        var message = MessageDocument.CreateInstance(
            request.QueueName,
            policy?.Version ?? 0, 
            request.CorrelationId,
            request.Content);

        // create message
        // insert
        // get objectid
        // publish to queue (entire object)
        // update message to published
        throw new NotImplementedException();
    }
}
