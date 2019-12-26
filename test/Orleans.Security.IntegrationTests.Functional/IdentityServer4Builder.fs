module IdentityServer4Builder

open System
open System.Net.Http
open IdentityModel.Client
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.DependencyInjection

let private buildTestServer () =
    let builder = 
        WebHostBuilder()
            .UseUrls("http://localhost:5001")
            .ConfigureServices(fun services -> 
                services.AddIdentityServer()
                        .AddDeveloperSigningCredential()
                        .AddInMemoryApiResources(IdentityServer4Config.getApiResources())
                        .AddInMemoryIdentityResources(IdentityServer4Config.getIdentityResources())
                        .AddInMemoryClients(IdentityServer4Config.getClients())
                        .AddTestUsers(IdentityServer4Config.getUsers()) |> ignore

                services.AddMvcCore() |> ignore)
            .Configure(fun app ->
                app.UseIdentityServer() |> ignore
                app.UseRouting() |> ignore
                app.UseEndpoints(fun endpoints -> ()) |> ignore)

    let identityServer4 = new TestServer(builder)
    identityServer4.CreateHandler() |> ignore
    identityServer4.BaseAddress <- new Uri("http://localhost:5001")

    identityServer4

let private getDiscoveryDocument (client : HttpClient) = async {
        let! discoveryResponse = client.GetDiscoveryDocumentAsync() |> Async.AwaitTask
        
        let discoveryDocument = new Orleans.Security.AccessToken.DiscoveryDocumentShortInfo()
        discoveryDocument.IntrospectionEndpoint <- discoveryResponse.IntrospectionEndpoint
        discoveryDocument.Issuer <- discoveryResponse.Issuer
        discoveryDocument.Keys <- discoveryResponse.KeySet.Keys
        discoveryDocument.TokenEndpoint <- discoveryResponse.TokenEndpoint

        return discoveryDocument
    }

let private identityServer4 = buildTestServer()
let private identityServer4Client = identityServer4.CreateClient()

let public discoveryDocument = identityServer4Client |>  getDiscoveryDocument |> Async.RunSynchronously

let requestClientCredentialsTokenAsync (clientId:string) (clientSecret:string) (scope:string) =
    let tokenRequest = new ClientCredentialsTokenRequest(Address = discoveryDocument.TokenEndpoint, Scope = scope,
                        ClientId = clientId, ClientSecret = clientSecret)
    
    let tokenResponse = identityServer4Client.RequestClientCredentialsTokenAsync(tokenRequest)
    tokenResponse
            

