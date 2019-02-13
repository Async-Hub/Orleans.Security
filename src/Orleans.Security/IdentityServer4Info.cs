namespace Orleans.Security
{
    public class IdentityServer4Info
    {
        public IdentityServer4Info(string url,
            string clientId, string clientSecret, string allowedScope,
            string discoveryEndpointEndpointPath = ".well-known/openid-configuration")
        {
            Url = url;
            ClientId = clientId;
            ClientSecret = clientSecret;
            AllowedScope = allowedScope;
            DiscoveryEndpointUrl = Url + "/" + discoveryEndpointEndpointPath;
        }

        public string Url { get; }

        public string ClientId { get; }

        public string ClientSecret { get; }

        public string AllowedScope { get; }

        public string DiscoveryEndpointUrl { get; }
    }
}