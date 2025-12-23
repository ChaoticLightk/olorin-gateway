using Microsoft.AspNetCore.Builder;
using MiddleR;

namespace Gateway.Domain;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddDomain(this WebApplicationBuilder builder)
    {
        builder.Services.AddMiddleR(AppDomain.CurrentDomain.GetAssemblies());
        return builder;
    }
}
