using Microsoft.AspNetCore.Http;
using Package.ResultPattern.ErrorResult;

namespace Package.ResultPattern.AspNetCore.MinimalApi;

public static partial class Extensions 
{
    private static IResult HandleMinimalSuccess() => Results.NoContent();

    private static IResult HandleMinimalError(this Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return error.Type switch
        {
            ErrorType.IncorrectArgument => Results.BadRequest((ErrorResponse)error),
            ErrorType.NotFound => Results.NotFound((ErrorResponse)error),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Forbidden => Results.StatusCode(StatusCodes.Status403Forbidden),
            ErrorType.InternalServer => Results.Problem(statusCode: StatusCodes.Status500InternalServerError),
            ErrorType.NotImplemented => Results.StatusCode(StatusCodes.Status501NotImplemented),
            ErrorType.FailedDependency => Results.StatusCode(StatusCodes.Status424FailedDependency),
            ErrorType.ServiceUnavailable => Results.StatusCode(StatusCodes.Status503ServiceUnavailable),
            ErrorType.RequestTimeOut => Results.StatusCode(StatusCodes.Status408RequestTimeout),
            _ => throw new InvalidOperationException("Não foi possível executar a função")
        };
    }

    public static IResult ToMinimalResult(this Result result) 
        => result.Match(
            onSuccess: HandleMinimalSuccess,
            HandleMinimalError
        ); 

    public static IResult ToMinimalResult<T>(this Result<T> result) 
        => result.Match(
            onSuccess: Results.Ok,
            HandleMinimalError
        );

    public static async Task<IResult> ToMinimalResult(this Task<Result> result) 
        => (await result).ToMinimalResult(); 

    public static async Task<IResult> ToMinimalResult<T>(this Task<Result<T>> result) 
        => (await result).ToMinimalResult();
}
