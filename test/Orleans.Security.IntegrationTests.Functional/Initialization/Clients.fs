module Clients

open IdentityModel
open IdentityServer4.Models
open System.Collections.Generic
open System.Security.Claims

let getClients() =
    let client = Client()
    client.ClientId <- "WebClient"

    client.AccessTokenType <- AccessTokenType.Jwt
    client.AllowedGrantTypes <- GrantTypes.ResourceOwnerPasswordAndClientCredentials
    client.AllowOfflineAccess <- true
    client.AllowedScopes.Add("Api1")
    client.AllowedScopes.Add("Orleans")
    client.AllowedScopes.Add(JwtClaimTypes.Role)
    client.Claims <- List<Claim> [ Claim(JwtClaimTypes.Role, "Admin") ]

    Secret(HashExtensions.Sha256 "Secret") |> client.ClientSecrets.Add

    [ client ] |> ResizeArray<Client>
