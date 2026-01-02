using Microsoft.AspNetCore.Builder;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.FluentValidation;

[assembly: WolverineModule]

namespace Gateway.Domain;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddDomain(this WebApplicationBuilder builder)
    {
        builder.Host.UseWolverine(opts =>
        {
            opts.UseFluentValidation();
        });

        return builder;
    }
}
