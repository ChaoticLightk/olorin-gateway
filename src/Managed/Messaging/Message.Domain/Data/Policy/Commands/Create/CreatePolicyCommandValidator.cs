using FluentValidation;
using Messaging.Domain.Data.Policy.Commands.DTO;

namespace Messaging.Domain.Data.Policy.Commands.Create;

public class CreatePolicyCommandValidator : AbstractValidator<PolicyQueueData>
{
    public CreatePolicyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
    }
}
