using Colombo.ResultPattern;
using Colombo.ResultPattern.ErrorResult;
using FluentValidation;

namespace Messaging.Domain.Chain.Exceptions.Handlers;

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
