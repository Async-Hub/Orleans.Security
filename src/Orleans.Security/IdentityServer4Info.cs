namespace Orleans.Security
{
    public class IdentityServer4Info
    {
        public IdentityServer4Info(string url,
            string clientScopeName, string clientSecret,
            string TokenIntrospectionEndpointPath = "connect/introspect")
        {
            Url = url;
            ClientScopeName = clientScopeName;
            ClientSecret = clientSecret;
            TokenIntrospectionEndpointUrl = Url + "/" + TokenIntrospectionEndpointPath;
        }

        public string Url { get; }

        public string ClientScopeName { get; }

        public string ClientSecret { get; }

        public string TokenIntrospectionEndpointUrl { get; }
    }
}