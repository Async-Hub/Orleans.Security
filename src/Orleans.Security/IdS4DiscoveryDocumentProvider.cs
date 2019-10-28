using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Orleans.Security.AccessToken;

namespace Orleans.Security
{
    internal class IdS4DiscoveryDocumentProvider
    {
        private readonly HttpClient _client;

        private readonly string _discoveryEndpointUrl;

        private DiscoveryDocumentShortInfo _discoveryDocument;

        internal IdS4DiscoveryDocumentProvider(IHttpClientFactory clientFactory, string discoveryEndpointUrl)
        {
            _client = clientFactory.CreateClient("IdS4");
            _discoveryEndpointUrl = discoveryEndpointUrl;
        }

        public async Task<DiscoveryDocumentShortInfo> GetDiscoveryDocumentAsync()
        {
            if (_discoveryDocument != null)
            {
                return _discoveryDocument;
            }

            var discoveryResponse = await _client.GetDiscoveryDocumentAsync(_discoveryEndpointUrl);

            if (discoveryResponse.IsError)
            {
                throw new Exception(discoveryResponse.Error);
            }

            _discoveryDocument = new DiscoveryDocumentShortInfo
            {
                IntrospectionEndpoint = discoveryResponse.IntrospectionEndpoint,
                Issuer = discoveryResponse.Issuer,
                Keys = discoveryResponse.KeySet.Keys
                
            };

            return _discoveryDocument;
        }
    }
}