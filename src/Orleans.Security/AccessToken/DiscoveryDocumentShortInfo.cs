using System.Collections.Generic;
using IdentityModel.Jwk;

namespace Orleans.Security.AccessToken
{
    public class DiscoveryDocumentShortInfo
    {
        public string IntrospectionEndpoint { get; set; }

        public IList<JsonWebKey> Keys { get; set; }

        public string Issuer { get; set; }

        public string TokenEndpoint { get; set; }
    }
}