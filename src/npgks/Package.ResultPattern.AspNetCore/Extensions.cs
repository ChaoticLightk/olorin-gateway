using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Package.ResultPattern.ErrorResult;

namespace Package.ResultPattern.AspNetCore;

public static partial class Extensions
{
    private static ActionResult HandleSuccess() => new NoContentResult();

    public static ActionResult<T> HandleSuccess<T>(this T value)
    {
        if (value is null)
            return HandleSuccess();

        return new OkObjectResult(value);
    }

    private static ActionResult HandleError(this Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return error.Type switch
        {
            ErrorType.IncorrectArgument => new BadRequestObjectResult((ErrorResponse)error),
            ErrorType.NotFound => new NotFoundObjectResult((ErrorResponse)error),
            ErrorType.Unauthorized => new UnauthorizedObjectResult((ErrorResponse)error),
            ErrorType.Forbidden => InstanceObjectResult(error, HttpStatusCode.Forbidden),
            ErrorType.InternalServer => InstanceObjectResult(error, HttpStatusCode.InternalServerError),
            ErrorType.NotImplemented => InstanceObjectResult(error, HttpStatusCode.NotImplemented),
            ErrorType.FailedDependency => InstanceObjectResult(error, HttpStatusCode.FailedDependency),
            ErrorType.ServiceUnavailable => InstanceObjectResult(error, HttpStatusCode.ServiceUnavailable),
            ErrorType.RequestTimeOut => InstanceObjectResult(error, HttpStatusCode.RequestTimeout),
            _ => throw new InvalidOperationException("Não foi possível executar a função")
        };
    }

    private static ObjectResult InstanceObjectResult(Error error, HttpStatusCode statusCode)
        => new((ErrorResponse) error)
            {
                StatusCode = (int)statusCode
            };

    public static ActionResult ToActionResult(this Result result) 
        => result.Match(
            onSuccess: HandleSuccess,
            onFailure: HandleError);

    public static ActionResult ToActionResult<T>(this Result<T> result) 
        => result.Match(
            onSuccess: HandleSuccess,
            onFailure: HandleError);

    public static async Task<ActionResult> ToActionResult(this Task<Result> result) 
        => (await result).ToActionResult();

    public static async Task<ActionResult<T>> ToActionResult<T>(this Task<Result<T>> result) 
        => (await result).ToActionResult<T>();
}