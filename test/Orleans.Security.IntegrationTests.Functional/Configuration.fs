module Configuration

open IdentityModel
open IdentityServer4.Models
open IdentityServer4.Test
open System.Collections.Generic;

    module IdentityServerConfig =
        open System.Security.Claims

        let getApiResources () = 
            let resources = List<ApiResource>()
            let api1 = ApiResource "Api1"
            resources.Add api1

            api1.UserClaims.Add JwtClaimTypes.Email
            api1.UserClaims.Add JwtClaimTypes.Role

            let secret = Secret (HashExtensions.Sha256 "Secret")
            api1.ApiSecrets.Add secret

            resources

        let getClients () =
            let clients = List<Client>()
            let client = Client()
            clients.Add client
            
            client.AccessTokenType <- AccessTokenType.Jwt
            client.AllowedGrantTypes <- GrantTypes.ClientCredentials
            client.AllowOfflineAccess <- true
            
            let scopes = List<string>()
            scopes.Add  "Api1"
            client.AllowedScopes <- scopes
            
            let secret = Secret (HashExtensions.Sha256 "Secret")
            client.ClientId <- "WebClient"
            client.ClientSecrets.Add secret

            clients

        let getIdentityResources () =
            let resources = List<IdentityResource>()

            resources.Add (IdentityResources.Email())
            resources.Add (IdentityResources.Profile())
            resources.Add (IdentityResources.OpenId())

            resources

        let getUsers () =
            let users = new List<TestUser>()

            let aliceClaims = [|
                new Claim(JwtClaimTypes.Name, "Alice Smith")
                new Claim(JwtClaimTypes.GivenName, "Alice")
                new Claim(JwtClaimTypes.FamilyName, "Smith")
                new Claim(JwtClaimTypes.Email, "AliceSmith@email.com")
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
                new Claim(JwtClaimTypes.WebSite, "http://alice.com")
                new Claim(JwtClaimTypes.Role, "Admin")
                new Claim(JwtClaimTypes.Role, "Manager")
            |]

            let alice = new TestUser(SubjectId = "1", Username = "alice", Password = "Pass123$", Claims = aliceClaims)

            let bobClaims = [|
                new Claim(JwtClaimTypes.Name, "Bob Smith")
                new Claim(JwtClaimTypes.GivenName, "Bob")
                new Claim(JwtClaimTypes.FamilyName, "Smith")
                new Claim(JwtClaimTypes.Email, "BobSmith@email.com")
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
                new Claim(JwtClaimTypes.WebSite, "http://bob.com")
                new Claim(JwtClaimTypes.Role, "Developer")
            |]

            let bob = new TestUser(SubjectId = "2", Username = "bob", Password = "Pass123$", Claims = bobClaims)

            users.Add alice
            users.Add bob
            users

open System
open System.Net.Http
open IdentityModel.Client
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.DependencyInjection

    module IdentityServer4Builder =
        let private buildTestServer () =
            let builder = 
                WebHostBuilder()
                    .UseUrls("http://localhost:5001")
                    .ConfigureServices(fun services -> 
                        services.AddIdentityServer()
                            .AddDeveloperSigningCredential()
                            .AddInMemoryApiResources(IdentityServerConfig.getApiResources())
                            .AddInMemoryIdentityResources(IdentityServerConfig.getIdentityResources())
                            .AddInMemoryClients(IdentityServerConfig.getClients())
                            .AddTestUsers(IdentityServerConfig.getUsers()) |> ignore

                        services.AddMvcCore() |> ignore)
                    .Configure(fun app ->
                        app.UseIdentityServer() |> ignore
                        app.UseMvcWithDefaultRoute() |> ignore)

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
            

