using FluentValidation;
using Package.ResultPattern;
using Package.ResultPattern.ErrorResult;

namespace Messaging.Domain.Chain.Handlers;

public class ValidationExceptionHandler
{
    public static Result Handle(ValidationException ex)
    {
        var errors = ex.Errors
            .Select(x => Error.BadRequest(x.ErrorMessage))
            .ToList();

        var message = string.Join(',', errors);
        
        return Error.BadRequest(message);
    }
}
