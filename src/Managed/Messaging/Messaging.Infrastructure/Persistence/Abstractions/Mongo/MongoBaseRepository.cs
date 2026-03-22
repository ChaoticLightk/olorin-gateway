using Package.ResultPattern;
using Package.ResultPattern.ErrorResult;

namespace Messaging.Infrastructure.Persistence.Abstractions.Mongo;

public abstract class MongoBaseRepository
{
    protected static async Task<Result<T>> ExecuteAsync<T>(
        Func<Task<T>> action,
        string errorMessage = ""
    )
    {
        try
        {
            return await action();            
        }
        catch (Exception ex)
        {
            return Error.InternalServer(errorMessage, ex: ex);
        }
    }

    protected static async Task<Result> ExecuteAsync(
        Func<Task> action,
        string errorMessage = ""
    )
    {
        try
        {
            await action();            
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Error.InternalServer(errorMessage, ex: ex);
        }
    }
}
