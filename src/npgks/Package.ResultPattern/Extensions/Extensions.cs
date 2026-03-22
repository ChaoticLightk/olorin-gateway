using Package.ResultPattern.ErrorResult;

namespace Package.ResultPattern.Extensions;

public static class Extensions
{
    public static async Task<T> Match<T>(this Task<Result> result, Func<T> onSuccess, Func<Error, T> onFailure)
        => (await result).Match(onSuccess, onFailure);

    public static async Task<R> Match<T, R>(this Task<Result<T>> result, Func<T, R> onSuccess, Func<Error, R> onFailure)
        => (await result).Match(onSuccess, onFailure);
}
