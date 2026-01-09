using FluentValidation;
using Messaging.Domain.Data.Policy.Commands.DTO;
using Messaging.Domain.Shared.Messaging.DTO;

namespace Messaging.Domain.Data.Policy.Commands.Create;

public class CreatePolicyCommandValidator : AbstractValidator<PolicyQueueData>
{
    public CreatePolicyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Retry)
            .NotNull()
            .When(x => x.QueueType is QueueType.Quorum);
    }
}
