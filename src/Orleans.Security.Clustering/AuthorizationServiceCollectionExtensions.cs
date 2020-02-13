using System;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Security.Authorization;

namespace Orleans.Security.Clustering
{
    public static class OrleansClusterSecurityServiceCollectionExtensions
    {
        // For the testing purposes.
        internal static void AddOrleansClusteringAuthorization(this IServiceCollection services,
            Action<Configuration> configure, Action<IServiceCollection> configureServices)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddSingleton<IIncomingGrainCallFilter, IncomingGrainCallAuthorizationFilter>();
            services.AddOrleansClusterSecurityServices(configure, configureServices);
        }

        // For the production usage.
        public static void AddOrleansClusteringAuthorization(this IServiceCollection services,
            IdentityServer4Info identityServer4Info, Action<Configuration> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (identityServer4Info == null)
            {
                throw new ArgumentNullException(nameof(identityServer4Info));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddSingleton(identityServer4Info);
            services.AddSingleton<IIncomingGrainCallFilter, IncomingGrainCallAuthorizationFilter>();
            services.AddOrleansClusterSecurityServices(configure);
        }
        
        public static void AddOrleansCoHostedClusterAuthorization(this IServiceCollection services,
            IdentityServer4Info identityServer4Info, Action<Configuration> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (identityServer4Info == null)
            {
                throw new ArgumentNullException(nameof(identityServer4Info));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddSingleton(identityServer4Info);
            services.AddSingleton<IOutgoingGrainCallFilter, AccessTokenSetterFilter>();
            services.AddSingleton<IIncomingGrainCallFilter, IncomingGrainCallAuthorizationFilter>();
            services.AddOrleansClusterSecurityServices(configure);
        }
    }
}