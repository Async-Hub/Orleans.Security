module IdentityServer4

open System
open System.Net.Http
open IdentityModel.Client
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.DependencyInjection
open Orleans.Security.AccessToken

let private run () =
    let builder = 
        WebHostBuilder()
            .UseUrls("http://localhost:5001")
            .ConfigureServices(fun services -> 
                services.AddIdentityServer()
                        .AddDeveloperSigningCredential()
                        .AddInMemoryApiResources(IdentityServer4Resources.getApiResources())
                        .AddInMemoryIdentityResources(IdentityServer4Resources.getIdentityResources())
                        .AddInMemoryClients(Clients.getClients())
                        .AddTestUsers(Users.getUsers()) |> ignore

                services.AddMvcCore() |> ignore)
            .Configure(fun app ->
                app.UseIdentityServer() |> ignore
                app.UseRouting() |> ignore
                app.UseEndpoints(fun endpoints -> ()) |> ignore)

    let identityServer4 = new TestServer(builder)
    identityServer4.CreateHandler() |> ignore
    identityServer4.BaseAddress <- new Uri("http://localhost:5001")

    identityServer4

let private getDiscoveryDocumentAsync (client : HttpClient) = async {
        let! discoveryResponse = client.GetDiscoveryDocumentAsync() |> Async.AwaitTask
        
        let discoveryDocument = DiscoveryDocumentShortInfo()
        discoveryDocument.IntrospectionEndpoint <- discoveryResponse.IntrospectionEndpoint
        discoveryDocument.Issuer <- discoveryResponse.Issuer
        discoveryDocument.Keys <- discoveryResponse.KeySet.Keys
        discoveryDocument.TokenEndpoint <- discoveryResponse.TokenEndpoint

        return discoveryDocument
    }

let private identityServer4Client = run().CreateClient()

let public discoveryDocument = identityServer4Client
                               |>  getDiscoveryDocumentAsync
                               |> Async.RunSynchronously

let requestClientCredentialsTokenAsync (clientId:string) (clientSecret:string) (scope:string) =
    let tokenRequest = new ClientCredentialsTokenRequest(
                        Address = discoveryDocument.TokenEndpoint, Scope = scope,
                        ClientId = clientId, ClientSecret = clientSecret)
    
    identityServer4Client.RequestClientCredentialsTokenAsync(tokenRequest)
            

