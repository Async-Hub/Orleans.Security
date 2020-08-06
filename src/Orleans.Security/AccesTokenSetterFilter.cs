using System;
using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.Security.Authorization;

namespace Orleans.Security
{
    internal class AccessTokenSetterFilter : IOutgoingGrainCallFilter
    {
        private readonly IAccessTokenProvider _accessTokenProvider;

        public AccessTokenSetterFilter(IAccessTokenProvider accessTokenProvider)
        {
            _accessTokenProvider = accessTokenProvider;
        }

        public async Task Invoke(IOutgoingGrainCallContext context)
        {
            if (AuthorizationAdmission.IsRequired(context))
            {
                var accessToken = RequestContext.Get(ConfigurationKeys.AccessTokenKey)?.ToString();

                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    accessToken = await _accessTokenProvider.RetrieveTokenAsync();

                    if (string.IsNullOrWhiteSpace(accessToken))
                    {
                        throw new InvalidOperationException("AccessToken can not be null or empty.");
                    }

                    RequestContext.Set(ConfigurationKeys.AccessTokenKey, accessToken);
                }
            }

            await context.Invoke();
        }
    }
}