namespace Orleans.Security

open System
open System.Linq
open System.Net.Http
open Orleans.Security.AccessToken
open IdentityModel.Client

//module RuntimeMethodBinder =
//    let invokeAsync (fullyQualifiedNameOfType: string) (nameOfMethod: string) (arguments: obj []) (argumentsCount: int) =
//        let typeVal = Type.GetType(fullyQualifiedNameOfType, true)
//        let methodQueryPredicate = typeVal.GetMethods().Where(fun m -> m.Name.Contains(nameOfMethod))
//        let method = methodQueryPredicate.Where(fun m -> m.GetParameters().Length = argumentsCount).FirstOrDefault()

//        if method = null then
//            let message = "The appropriate method " + nameOfMethod + "doesn't find in IdentityModel package."
//            raise (InvalidOperationException(message))


type IdS4DiscoveryDocumentProvider(clientFactory: IHttpClientFactory,
                                   discoveryEndpointUrl: string,
                                   securityOptions: SecurityOptions) =
    let httpClient = clientFactory.CreateClient("IdS4")
    let mutable discoveryDocument: DiscoveryDocumentShortInfo = null

    member this.GetDiscoveryDocumentAsync() =
        async {
            if not (isNull discoveryDocument) then
                return discoveryDocument
            else
                let request = new DiscoveryDocumentRequest()
                request.Address <- discoveryEndpointUrl
                request.Policy.RequireHttps <- securityOptions.RequireHttps
                
                let! discoveryResponse = httpClient.GetDiscoveryDocumentAsync(request) |> Async.AwaitTask
                if discoveryResponse.IsError then raise (Exception(discoveryResponse.Error))

                let discoveryDocument = DiscoveryDocumentShortInfo()
                discoveryDocument.IntrospectionEndpoint <- discoveryResponse.IntrospectionEndpoint
                discoveryDocument.Issuer <- discoveryResponse.Issuer
                discoveryDocument.Keys <- discoveryResponse.KeySet.Keys

                return discoveryDocument
        }
        |> Async.StartAsTask

//using System;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using IdentityModel.Client;
//using Orleans.Security.AccessToken;

//namespace Orleans.Security
//{
//    internal class IdS4DiscoveryDocumentProvider
//    {
//        private readonly HttpClient _client;

//        private readonly string _discoveryEndpointUrl;

//        private DiscoveryDocumentShortInfo _discoveryDocument;

//        internal IdS4DiscoveryDocumentProvider(IHttpClientFactory clientFactory, string discoveryEndpointUrl)
//        {
//            _client = clientFactory.CreateClient("IdS4");
//            _discoveryEndpointUrl = discoveryEndpointUrl;
//        }

//        public async Task<DiscoveryDocumentShortInfo> GetDiscoveryDocumentAsync()
//        {
//            if (_discoveryDocument != null)
//            {
//                return _discoveryDocument;
//            }

//            // TODO: This approach should be reconsidered in future.
//            /*
//            The solution below allows use "Orleans.Security" with IdentityServer4 v2.x and IdentityServer4 v3.x.
//            At the same time, DLR with Reflection in is a bad idea.
//            */
//            const string fullyQualifiedNameOfType = "IdentityModel.Client.HttpClientDiscoveryExtensions, IdentityModel";

//            var cancellationToken = default(CancellationToken);
//            var param = new object[] {_client, _discoveryEndpointUrl, cancellationToken};

//            // ReSharper disable once SuggestVarOrType_SimpleTypes
//            dynamic discoveryResponse =
//                await RuntimeMethodBinder.InvokeAsync(fullyQualifiedNameOfType,
//                    "GetDiscoveryDocumentAsync", param, 3);

//            // TODO: This should be used normally.
//            //var discoveryResponse = await _client.GetDiscoveryDocumentAsync(_discoveryEndpointUrl);

//            if (discoveryResponse.IsError)
//            {
//                throw new Exception(discoveryResponse.Error);
//            }

//            _discoveryDocument = new DiscoveryDocumentShortInfo
//            {
//                IntrospectionEndpoint = discoveryResponse.IntrospectionEndpoint,
//                Issuer = discoveryResponse.Issuer,
//                Keys = discoveryResponse.KeySet.Keys

//            };

//            return _discoveryDocument;
//        }
//    }
//}