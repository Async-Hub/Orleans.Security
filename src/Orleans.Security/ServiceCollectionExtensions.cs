using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans.Security.AccessToken;
using Orleans.Security.Authorization;
using Orleans.Security.Caching;

namespace Orleans.Security
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddOrleansClusterSecurityServices(this IServiceCollection services,
            Action<Configuration> configure,
            Action<IServiceCollection> configureServices = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var configuration = new Configuration();
            configure.Invoke(configuration);

            services.AddAuthorization(configuration.ConfigureAuthorizationOptions);

            services.TryAdd(ServiceDescriptor.Singleton<IAuthorizeHandler, AuthorizeHandler>());

            configureServices?.Invoke(services);

            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (!configuration.TracingEnabled)
            {
                services.TryAdd(ServiceDescriptor.Singleton<IAccessTokenVerifier, AccessTokenVerifier>());
            }
            else
            {
                services.TryAdd(ServiceDescriptor.Singleton<IAccessTokenVerifier, AccessTokenVerifierWithTracing>());
            }

            var accessTokenVerifierOptions = new AccessTokenVerifierOptions();

            configuration.ConfigureAccessTokenVerifierOptions?.Invoke(accessTokenVerifierOptions);
            services.Add(ServiceDescriptor.Singleton(accessTokenVerifierOptions));

            var memoryCacheOptions = new MemoryCacheOptions();
            services.AddSingleton<IAccessTokenCache>(serviceProvider => new AccessTokenCache(memoryCacheOptions));
        }
    }
}