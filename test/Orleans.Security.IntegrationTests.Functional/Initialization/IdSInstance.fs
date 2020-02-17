module IdSInstance

open IdentityModel.Client
open Orleans.Security.AccessToken
open System
open System.Net.Http

let private getDiscoveryDocumentAsync (client: HttpClient) =
    async {
        let! discoveryResponse = client.GetDiscoveryDocumentAsync() |> Async.AwaitTask

        let discoveryDocument = DiscoveryDocumentShortInfo()
        discoveryDocument.IntrospectionEndpoint <- discoveryResponse.IntrospectionEndpoint
        discoveryDocument.Issuer <- discoveryResponse.Issuer
        discoveryDocument.Keys <- discoveryResponse.KeySet.Keys
        discoveryDocument.TokenEndpoint <- discoveryResponse.TokenEndpoint

        return discoveryDocument
    }

let client =
    IdentityServer.startServer() |> ignore
    let httpClient = new HttpClient()
    httpClient.BaseAddress <- Uri GlobalConfig.identityServer4Url
    httpClient

let public discoveryDocument = client |> getDiscoveryDocumentAsync |> Async.RunSynchronously