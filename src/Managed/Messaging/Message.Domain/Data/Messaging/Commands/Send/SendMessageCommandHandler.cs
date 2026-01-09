using Package.ResultPattern;
using Package.ResultPattern.ErrorResult;
using Messaging.Domain.Data.Abstractions.Wolwerine;
using Messaging.Domain.Data.Messaging.Persistence.Abstractions;
using Messaging.Domain.Data.Policy.Persistence.Abstractions;
using Messaging.Domain.Entities.Queue.Enums;
using Messaging.Domain.Entities.Queue.Mongo;
using Messaging.Domain.Shared.Messaging.DTO;
using Messaging.Domain.Shared.Messaging.Services;

namespace Messaging.Domain.Data.Messaging.Commands.Send;

public class SendMessageCommandHandler(
    IQueueService queue,
    IQueueMessageRepository messageRepository,
    IQueuePolicyRepository policyRepository) : IHandler<SendMessageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        SendMessageCommand request,
        CancellationToken cancellationToken = default)
    {
        var policyResult = await policyRepository
            .Get(request.QueueName, cancellationToken);

        if (policyResult.IsFailure)
        {
            return policyResult.Error;
        }

        var policy = policyResult.Value;

        if (policy is null)
        {
            return Error.BadRequest($"A fila {request.QueueName} não existe no cadastro do barramento.");
        }

        var messageDocument = MessageDocument.CreateInstance(
            request.QueueName,
            policy.Version,
            request.CorrelationId,
            request.Payload);

        var messageResult = await messageRepository
            .CreateMessage(messageDocument, cancellationToken);

        if (messageResult.IsFailure)
        {
            return messageResult.Error;
        }

        var message = messageResult.Value;

        if (message is null)
        {
            return Error.InternalServer("A mensagem criada não foi retornada, tente novamente mais tarde.");
        }

        var queueMessage = Message.CreateInstance(
            message.Queue,
            message.PolicyVersion,
            message.State,
            message.RetryCount,
            request.Payload
        );

        try
        {
            await queue.PublishAsync(queueMessage, ct: cancellationToken);
        }
        catch (Exception ex)
        {
            return Error.InternalServer("Ocorreu um erro ao encaminhar a mesagem para a fila: ", ex: ex);
        }

        message.SetState(MessageState.Published);

        var updateResult = await messageRepository
            .UpdateMessage(message, cancellationToken);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        return message.Id;    
    }
}
