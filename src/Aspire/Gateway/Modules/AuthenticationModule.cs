using System.Text;
using Gateway.Application.Authentication.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.Modules;

public static class AuthenticationModule
{
    public static readonly string BASIC_AUTH_SCHEME = "Basic";

    public static WebApplicationBuilder AddAuthenticationModule(this WebApplicationBuilder builder)
    {
        string? key = builder.Configuration["JWT:Key"];

        ArgumentNullException.ThrowIfNull(key);

        byte[] byteKey = Encoding.UTF8.GetBytes(key);

        builder.Services
            .AddAuthentication()
            .AddJwtBearer(a =>
            {
                a.SaveToken = true;
                a.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(byteKey)
                };
            })
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
                BASIC_AUTH_SCHEME,
                null);

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(a =>
        {
            a.SaveToken = true;
            a.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(byteKey)
            };
        });

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
