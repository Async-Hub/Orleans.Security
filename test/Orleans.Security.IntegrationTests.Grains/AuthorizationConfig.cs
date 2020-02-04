using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains
{
    // ReSharper disable once UnusedType.Global
    public static class AuthorizationConfig
    {
        // ReSharper disable once UnusedMember.Global
        public static void ConfigureOptions(AuthorizationOptions options)
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
            options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
        }
    }
}
