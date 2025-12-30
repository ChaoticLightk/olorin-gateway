using Microsoft.AspNetCore.Builder;
using Wolverine;
using Wolverine.Attributes;

[assembly: WolverineModule]

namespace Messaging.Domain;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddDomain(this WebApplicationBuilder builder)
    {
        builder.Host.UseWolverine();
        return builder;
    }
}