using System;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Security.Authorization;

namespace Orleans.Security.ClusterClient
{
    public static class OrleansClusterSecurityServiceCollectionExtensions
    {
        // For the testing purposes.
        internal static void AddOrleansClusterAuthorization(this IServiceCollection services,
            Action<AuthorizationOptions> configure, Action<IServiceCollection> configureServices)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddServices(configure);
            services.AddOrleansClusterSecurityServices(configureServices);
        }

        // For the production usage.
        public static void AddOrleansClusterAuthorization(this IServiceCollection services,
            Action<AuthorizationOptions> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddServices(configure);
        }

        private static void AddServices(this IServiceCollection services, Action<AuthorizationOptions> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddAuthorization(configure);
            services.AddOrleansClusterSecurityServices();
            services.AddSingleton<IOutgoingGrainCallFilter, OutgoingGrainCallAuthorizationFilter>();
        }
    }
}