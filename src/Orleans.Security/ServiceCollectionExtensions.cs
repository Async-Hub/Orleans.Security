using System;
using System.Net.Http;
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

            services.TryAdd(ServiceDescriptor.Singleton<IAuthorizationExecutor, AuthorizationExecutor>());

            configureServices?.Invoke(services);

            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (!configuration.TracingEnabled)
            {
                services.TryAdd(ServiceDescriptor.Singleton<IAccessTokenVerifier, DefaultAccessTokenVerifier>());
            }
            else
            {
                services.TryAdd(ServiceDescriptor.Singleton<IAccessTokenVerifier, AccessTokenVerifierWithTracing>());
            }

            var accessTokenVerifierOptions = new AccessTokenVerifierOptions();

            services.AddScoped<IAccessTokenIntrospectionService, DefaultAccessTokenIntrospectionService>();

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

            configuration.ConfigureAccessTokenVerifierOptions?.Invoke(accessTokenVerifierOptions);
            services.Add(ServiceDescriptor.Singleton(accessTokenVerifierOptions));

            var memoryCacheOptions = new MemoryCacheOptions();
            services.AddSingleton<IAccessTokenCache>(serviceProvider => new AccessTokenCache(memoryCacheOptions));
            services.AddSingleton(provider => new
                IdS4DiscoveryDocumentProvider(provider.GetRequiredService<IHttpClientFactory>(),
                    provider.GetRequiredService<IdentityServer4Info>().DiscoveryEndpointUrl));
        }
    }
}