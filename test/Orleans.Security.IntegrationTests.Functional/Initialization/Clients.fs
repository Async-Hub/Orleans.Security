module Clients

open IdentityServer4.Models

let getClients() =
    let client = Client()
    client.ClientId <- "WebClient"
    
    client.AccessTokenType <- AccessTokenType.Jwt
    client.AllowedGrantTypes <- GrantTypes.ClientCredentials
    client.AllowOfflineAccess <- true
    client.AllowedScopes.Add("Api1")

    Secret(HashExtensions.Sha256 "Secret") |> client.ClientSecrets.Add

    [ client ] |> ResizeArray<Client>
