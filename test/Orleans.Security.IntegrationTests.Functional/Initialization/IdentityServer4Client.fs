module IdentityServer4Client

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

let private identityServer4Client =
    IdentityServer4.startServer() |> ignore
    let httpClient = new HttpClient()
    httpClient.BaseAddress <- Uri GlobalConfig.identityServer4Url
    httpClient

let public discoveryDocument =
    identityServer4Client
    |> getDiscoveryDocumentAsync
    |> Async.RunSynchronously

let getAccessTokenAsync (clientId: string) (clientSecret: string) (scope: string) =
    let tokenRequest =
        new ClientCredentialsTokenRequest(Address = discoveryDocument.TokenEndpoint,
                                          Scope = scope, ClientId = clientId,
                                          ClientSecret = clientSecret)

    identityServer4Client.RequestClientCredentialsTokenAsync(tokenRequest)
    
let getAccessTokenWAsync (userName: string) (password: string) (scope: string) =
    let passwordTokenRequest = new PasswordTokenRequest(
                                Address = discoveryDocument.TokenEndpoint,
                                ClientId = "WebClient",
                                ClientSecret = "Secret",
                                UserName = userName,
                                Password = password,
                                Scope = scope)

    identityServer4Client.RequestPasswordTokenAsync(passwordTokenRequest);

