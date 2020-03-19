namespace Orleans.Security

open System
open System.Linq
open System.Net.Http
open Orleans.Security.AccessToken
open IdentityModel.Client

module RuntimeMethodBinder =
    let invokeAsync (fullyQualifiedNameOfType: string) (nameOfMethod: string) (arguments: obj []) (argumentsCount: int) =
        let typeVal = Type.GetType(fullyQualifiedNameOfType, true)
        let methodQueryPredicate = typeVal.GetMethods().Where(fun m -> m.Name.Contains(nameOfMethod))
        let method = methodQueryPredicate.Where(fun m -> m.GetParameters().Length = argumentsCount).FirstOrDefault()

        if method = null then
            let message = "The appropriate method " + nameOfMethod + "doesn't find in IdentityModel package."
            raise (InvalidOperationException(message))


type IdS4DiscoveryDocumentProvider(clientFactory: IHttpClientFactory,
                                   discoveryEndpointUrl: string) =
    let httpClient = clientFactory.CreateClient("IdS4")
    let mutable discoveryDocument: DiscoveryDocumentShortInfo = null

    member this.GetDiscoveryDocumentAsync() =
        async {
            if discoveryDocument <> null then
                return discoveryDocument
            else
                let! discoveryResponse = httpClient.GetDiscoveryDocumentAsync(discoveryEndpointUrl) |> Async.AwaitTask
                if discoveryResponse.IsError then raise (Exception(discoveryResponse.Error))

                let discoveryDocument = DiscoveryDocumentShortInfo()
                discoveryDocument.IntrospectionEndpoint <- discoveryResponse.IntrospectionEndpoint
                discoveryDocument.Issuer <- discoveryResponse.Issuer
                discoveryDocument.Keys <- discoveryResponse.KeySet.Keys

                return discoveryDocument
        }
        |> Async.StartAsTask
