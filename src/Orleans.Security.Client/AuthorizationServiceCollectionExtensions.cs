using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans.Security.Authorization;

namespace Orleans.Security.Client
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

            services.TryAddSingleton<IOutgoingGrainCallFilter, AccessTokenSetterFilter>();
            services.TryAddSingleton<IOutgoingGrainCallFilter, OutgoingGrainCallAuthorizationFilter>();
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
            services.AddSingleton<IOutgoingGrainCallFilter, AccessTokenSetterFilter>();
            services.AddSingleton<IOutgoingGrainCallFilter, OutgoingGrainCallAuthorizationFilter>();
            services.AddOrleansClusterSecurityServices(configure);
        }
    }
}