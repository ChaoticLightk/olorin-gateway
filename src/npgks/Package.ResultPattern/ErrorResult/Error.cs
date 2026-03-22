namespace Package.ResultPattern.ErrorResult;

public record Error(
    string Message,
    ErrorType Type,
    string? EventId = default,
    Exception? InnerException = null)
{
    public static Error InvalidArgument(string message, string? eventId = default, Exception? ex = null)
        => new(message, ErrorType.IncorrectArgument, eventId ,ex);

    public static Error BadRequest(string message, string? eventId = default, Exception? ex = null)
        => new(message, ErrorType.BadRequest, eventId, ex);

    public static Error NotFound(string message, string? eventId = default, Exception? ex = null)
        => new(message, ErrorType.NotFound, eventId, ex);

    public static Error Unauthorized(string message, string? eventId = default, Exception? ex = null)
       => new(message, ErrorType.Unauthorized, eventId, ex);

    public static Error Forbidden(string message, string? eventId = default, Exception? ex = null)
       => new(message, ErrorType.Forbidden, eventId, ex);

    public static Error Conflict(string message, string? eventId = default, Exception? ex = null)
        => new(message, ErrorType.Conflict, eventId, ex);

    public static Error FailedDependency(string message, string? eventId = default, Exception? ex = null)
       => new(message, ErrorType.FailedDependency, eventId, ex);

    public static Error InternalServer(string message, string? eventId = default, Exception? ex = null)
       => new(message, ErrorType.InternalServer, eventId, ex);
}
