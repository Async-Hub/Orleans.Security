using System;
using System.Net.Http;
using System.Threading;
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

            // TODO: This approach should be reconsidered in future.
            /*
            The solution below allows use "Orleans.Security" with IdentityServer4 v2.x and IdentityServer4 v3.x.
            At the same time, DLR with Reflection in is a bad idea.
            */
            const string fullyQualifiedNameOfType = "IdentityModel.Client.HttpClientDiscoveryExtensions, IdentityModel";

            var cancellationToken = default(CancellationToken);
            var param = new object[] {_client, _discoveryEndpointUrl, cancellationToken};

            // ReSharper disable once SuggestVarOrType_SimpleTypes
            dynamic discoveryResponse =
                await RuntimeMethodBinder.InvokeAsync(fullyQualifiedNameOfType,
                    "GetDiscoveryDocumentAsync", param, 3);

            // TODO: This should be used normally.
            //var discoveryResponse = await _client.GetDiscoveryDocumentAsync(_discoveryEndpointUrl);

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