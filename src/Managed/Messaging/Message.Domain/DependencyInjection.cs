using Microsoft.AspNetCore.Builder;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.FluentValidation;

[assembly: WolverineModule]

namespace Messaging.Domain;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddDomain(this WebApplicationBuilder builder)
    {
        builder.Host.UseWolverine(otps =>
        {
            otps.UseFluentValidation();
            // otps.Policies.AddMiddleware<ValidationMiddleware>();
        });

        return builder;
    }
}