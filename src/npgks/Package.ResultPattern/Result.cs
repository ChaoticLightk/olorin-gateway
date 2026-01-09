using System.Diagnostics.CodeAnalysis;
using Package.ResultPattern.ErrorResult;

namespace Package.ResultPattern;

public record Result(bool Success, Error? Error)
{
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; init; } = Success;

    public static Result Ok() => new(true, null);
    public static Result Fail(Error error) => new(false, error);

    public T Match<T>(Func<T> onSuccess, Func<Error, T> onFailure)
        => IsSuccess switch
        {
            true => onSuccess(),
            false => onFailure(Error)
        };

    public static implicit operator Result(Error error) => Fail(error);
}

public record Result<T>(bool IsSuccess, Error? Error, T? Value) 
    : Result(IsSuccess, Error)
{
    public static Result<T> Ok(T value) => new(true, null, value);
    public static new Result<T> Fail(Error error) => new(false, error, default);

    public R Match<R>(Func<T, R> onSuccess, Func<Error, R> onFailure)
        => IsSuccess switch
        {
            true => onSuccess(Value!),
            false => onFailure(Error)
        };

    public Result<R> Then<R>(Func<T, R> mapper)
        => IsSuccess switch
        {
            true => mapper(Value!),
            false => Error
        };

    public static implicit operator Result<T>(T value) => Ok(value);
    public static implicit operator Result<T>(Error error) => Fail(error);
}

