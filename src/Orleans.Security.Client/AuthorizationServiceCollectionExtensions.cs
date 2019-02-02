using System;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddSingleton<IOutgoingGrainCallFilter, OutgoingGrainCallAuthorizationFilter>();
            services.AddOrleansClusterSecurityServices(configure, configureServices);
        }

        // For the production usage.
        public static void AddOrleansClusteringAuthorization(this IServiceCollection services,
            Action<Configuration> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddSingleton<IOutgoingGrainCallFilter, OutgoingGrainCallAuthorizationFilter>();
            services.AddOrleansClusterSecurityServices(configure);
        }
    }
}