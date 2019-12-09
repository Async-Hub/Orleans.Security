module IdentityServer4Config

open IdentityModel
open IdentityServer4.Models
open IdentityServer4.Test
open System.Security.Claims
open System.Collections.Generic;

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

