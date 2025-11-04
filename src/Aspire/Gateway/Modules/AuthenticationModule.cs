using Gateway.Application.Authentication.Handlers;
using Microsoft.AspNetCore.Authentication;

namespace Gateway.Modules;

public static class AuthenticationModule
{
    public static readonly string BASIC_AUTH_SCHEME = "Basic";

    public static WebApplicationBuilder AddAuthenticationModule(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(BASIC_AUTH_SCHEME)
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
                BASIC_AUTH_SCHEME,
                null);

        builder.Services
            .AddAuthorizationBuilder()
            .AddPolicy(BASIC_AUTH_SCHEME, policy =>
            {
                policy.RequireAuthenticatedUser();
            });

        return builder;
    }

    public static void ConfigureAuthentication(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
