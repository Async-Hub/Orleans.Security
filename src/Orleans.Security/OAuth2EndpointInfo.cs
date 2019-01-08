namespace Orleans.Security
{
    public class OAuth2EndpointInfo
    {
        public OAuth2EndpointInfo(string authorityUrl,
            string clientScopeName, string clientSecret)
        {
            AuthorityUrl = authorityUrl;
            ClientScopeName = clientScopeName;
            ClientSecret = clientSecret;
        }

        public string AuthorityUrl { get; }

        public string ClientScopeName { get; }

        public string ClientSecret { get; }
    }
}