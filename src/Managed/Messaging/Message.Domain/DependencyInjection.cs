using Microsoft.AspNetCore.Builder;
using Shared.Constants;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.FluentValidation;
using Wolverine.RabbitMQ;

[assembly: WolverineModule]

namespace Messaging.Domain;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddDomain(this WebApplicationBuilder builder)
    {
        builder.Host.UseWolverine(otps =>
        {
            otps.UseFluentValidation();

            otps.UseRabbitMqUsingNamedConnection(RabbitMQConfiguration.CONNECTION_NAME)
                .UseConventionalRouting(x =>
                {
                    x.ConfigureSending((x, c) =>
                    {
                    });
                });

            // otps.Policies.AddMiddleware<ValidationMiddleware>();
        });

        return builder;
    }
}