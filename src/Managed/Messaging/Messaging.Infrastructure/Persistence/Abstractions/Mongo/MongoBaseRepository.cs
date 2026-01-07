using Colombo.ResultPattern;
using Colombo.ResultPattern.ErrorResult;

namespace Messaging.Infrastructure.Persistence.Abstractions.Mongo;

public abstract class MongoBaseRepository
{
    protected async Task<Result<T>> ExecuteAsync<T>(
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

    protected async Task<Result> ExecuteAsync(
        Func<Task> action,
        string errorMessage = ""
    )
    {
        try
        {
            await action();            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Error.InternalServer(errorMessage, ex: ex);
        }
    }
}
