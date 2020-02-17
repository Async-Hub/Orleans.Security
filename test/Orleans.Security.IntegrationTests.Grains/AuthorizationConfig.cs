using Microsoft.Extensions.DependencyInjection;
using Orleans.Security.Authorization;
using Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization;

namespace Orleans.Security.IntegrationTests.Grains
{
    // ReSharper disable once UnusedType.Global
    public static class AuthorizationConfig
    {
        internal const string DocumentModifyAccessPolicy = "DocumentModifyAccess";
        
        // ReSharper disable once UnusedMember.Global
        public static void ConfigureOptions(AuthorizationOptions options)
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
            options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
            
            options.AddPolicy("DocRegistryAccess", 
                policy => policy.AddRequirements(new DocRegistryAccessRequirement()));
            
            options.AddPolicy(DocumentModifyAccessPolicy, 
                policy => policy.AddRequirements(new SameAuthorRequirement()));
        }
        
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, DocModifyAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, DocRegistryAccessHandler>();
        }
    }
}
