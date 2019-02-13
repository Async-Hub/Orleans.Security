using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Orleans.Security
{
    internal class IdS4DiscoveryDocumentProvider
    {
        private readonly HttpClient _client;

        private readonly string _discoveryEndpointUrl;

        private DiscoveryResponse _discoveryResponse;

        internal IdS4DiscoveryDocumentProvider(IHttpClientFactory clientFactory, string discoveryEndpointUrl)
        {
            _client = clientFactory.CreateClient("IdS4");
            _discoveryEndpointUrl = discoveryEndpointUrl;
        }

        public async Task<DiscoveryResponse> GetDiscoveryDocumentAsync()
        {
            if (_discoveryResponse != null)
            {
                return _discoveryResponse;
            }

            _discoveryResponse = await _client.GetDiscoveryDocumentAsync(_discoveryEndpointUrl);

            if (_discoveryResponse.IsError)
            {
                throw new Exception(_discoveryResponse.Error);
            }

            return _discoveryResponse;
        }
    }
}