using System;
using Microsoft.AspNetCore.Mvc;

namespace Package.ResultPattern.AspNetCore;

internal sealed class HttpResult(Result result) : ActionResult
{
    private readonly Result _result = result;

    public static implicit operator HttpResult(Result res) 
        => new(res);

    public override void ExecuteResult(ActionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _result
            .ToActionResult()
            .ExecuteResult(context);
    }

    public override Task ExecuteResultAsync(ActionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        return _result
            .ToActionResult()
            .ExecuteResultAsync(context);
    }
}

internal sealed class HttpResult<T>(Result<T> result) : ActionResult
{
    private readonly Result<T> _result = result;

    public static implicit operator HttpResult<T>(Result<T> res) 
        => new(res);

    public override void ExecuteResult(ActionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _result
            .ToActionResult()
            .ExecuteResult(context);
    }

    public override Task ExecuteResultAsync(ActionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        return _result
            .ToActionResult()
            .ExecuteResultAsync(context);
    }
}

