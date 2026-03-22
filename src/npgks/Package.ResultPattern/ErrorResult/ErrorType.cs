namespace Package.ResultPattern.ErrorResult;

public enum ErrorType
{
    IncorrectArgument,
    BadRequest,
    NotFound,
    Unauthorized,
    Conflict,
    Forbidden,
    InternalServer,
    NotImplemented,
    FailedDependency,
    ServiceUnavailable,
    RequestTimeOut
}
