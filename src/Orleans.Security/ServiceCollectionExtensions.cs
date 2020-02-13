using System;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
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

            services.TryAdd(ServiceDescriptor.Singleton<IAuthorizationExecutor, AuthorizationExecutor>());

            configureServices?.Invoke(services);

            // Access Token verification section.
            var accessTokenVerifierOptions = new AccessTokenVerifierOptions();
            configuration.ConfigureAccessTokenVerifierOptions?.Invoke(accessTokenVerifierOptions);
            services.Add(ServiceDescriptor.Singleton(accessTokenVerifierOptions));

            services.TryAddSingleton<DefaultAccessTokenVerifier>();
            services.AddTransient<IAccessTokenIntrospectionService, AccessTokenIntrospectionServiceDefault>();

            //TODO: Maybe there is a better solution to split configuration for testing purposes.
            // If the environment is not in testing mode.
            if (configureServices == null)
            {
                services.Add(
                    ServiceDescriptor.Describe(typeof(AccessTokenVerifierWithCaching), serviceProvider =>
                    {
                        var defaultAccessTokenVerifier = serviceProvider.GetService<DefaultAccessTokenVerifier>();
                        var accessTokenCache = serviceProvider.GetService<IAccessTokenCache>();

                        return new AccessTokenVerifierWithCaching(defaultAccessTokenVerifier,
                            accessTokenCache, accessTokenVerifierOptions.CacheEntryExpirationTime);

                    }, ServiceLifetime.Singleton));

                services.Add(ServiceDescriptor.Describe(typeof(IAccessTokenVerifier), serviceProvider =>
                    {
                        IAccessTokenVerifier service = serviceProvider.GetService<DefaultAccessTokenVerifier>();
                        var isCacheEnabled = accessTokenVerifierOptions.InMemoryCacheEnabled;

                        if (isCacheEnabled)
                        {
                            service = serviceProvider.GetService<AccessTokenVerifierWithCaching>();
                        }

                        if (!configuration.TracingEnabled)
                        {
                            return service;
                        }

                        var logger = serviceProvider.GetRequiredService<ILogger<IAccessTokenVerifier>>();
                        return new AccessTokenVerifierWithTracing(isCacheEnabled, service, logger);
                    }
                    , ServiceLifetime.Singleton));
            }

            services.AddHttpClient("IdS4").ConfigureHttpMessageHandlerBuilder(builder =>
            {
                var httpClientHandler = new HttpClientHandler();
                builder.PrimaryHandler = httpClientHandler;

                if (accessTokenVerifierOptions.DisableCertificateValidation)
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback +=
                        (sender, certificate, chain, sslPolicyErrors) =>
                        {
                            if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
                            {
                            }

                            return true;
                        };
                }
            });

            var memoryCacheOptions = new MemoryCacheOptions();
            services.AddSingleton<IAccessTokenCache>(serviceProvider => new AccessTokenCache(memoryCacheOptions));
            services.AddSingleton(provider => new
                IdS4DiscoveryDocumentProvider(provider.GetRequiredService<IHttpClientFactory>(),
                    provider.GetRequiredService<IdentityServer4Info>().DiscoveryEndpointUrl));
        }
    }
}