namespace Gateway.DependecyInjection;

public static class CorsModules
{
    private const string POLICY_NAME = "";
    private const int ONE_HOUR_IN_SECONDS = 3600;

    public static WebApplicationBuilder AddCorsModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(setup =>
        {
            setup.AddPolicy(POLICY_NAME, policy =>
            {
                policy.SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins([])
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(ONE_HOUR_IN_SECONDS));
                
                if (!builder.Environment.IsProduction())
                {
                    policy.SetIsOriginAllowed(origin => true);
                }
            });
        });

        return builder;
    }

    public static void ConfigureCorsModule(this IApplicationBuilder app)
    {
        app.UseCors(POLICY_NAME);
    }
}
