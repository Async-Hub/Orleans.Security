using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains
{
    public static class AuthorizationConfig
    {
        public static void ConfigureOptions(AuthorizationOptions options)
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
            options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
        }
    }
}
