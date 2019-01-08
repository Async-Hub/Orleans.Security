using System;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Security.Authorization;

namespace Orleans.Security.ClusterClient
{
    public static class OrleansClusterSecurityServiceCollectionExtensions
    {
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

            services.AddAuthorization(configure);

            services.AddOrleansClusterSecurityServices(configureServices);
        }

        private static void AddOrleansClusterAuthorization(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOrleansClusterSecurityServices();
        }

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

            services.AddAuthorization(configure);

            services.AddOrleansClusterAuthorization();
        }
    }
}