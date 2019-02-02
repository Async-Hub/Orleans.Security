using System;
using Orleans.Security.AccessToken;
using Orleans.Security.Authorization;

namespace Orleans.Security
{
    public class Configuration
    {
        public Action<AccessTokenVerifierOptions> ConfigureAccessTokenVerifierOptions { get; set; }

        public Action<AuthorizationOptions> ConfigureAuthorizationOptions { get; set; }

        public bool TracingEnabled { get; set; }
    }
}