module IdSClients

open IdentityServer4.Models

let getClients() =
    let webClient1 = Client()
    webClient1.ClientId <- GlobalConfig.WebClient1
    webClient1.AccessTokenType <- AccessTokenType.Jwt
    webClient1.AllowedGrantTypes <- GrantTypes.ResourceOwnerPasswordAndClientCredentials
    webClient1.AllowOfflineAccess <- true
    webClient1.AllowedScopes.Add("Api1")
    webClient1.AllowedScopes.Add("Orleans")
    
    let webClient2 = Client()
    webClient2.ClientId <- GlobalConfig.WebClient2
    webClient2.AccessTokenType <- AccessTokenType.Jwt
    webClient2.AllowedGrantTypes <- GrantTypes.ResourceOwnerPasswordAndClientCredentials
    webClient2.AllowOfflineAccess <- true
    webClient2.AllowedScopes.Add("Api1")

    Secret(HashExtensions.Sha256 "Secret1") |> webClient1.ClientSecrets.Add
    Secret(HashExtensions.Sha256 "Secret2") |> webClient2.ClientSecrets.Add

    [ webClient1; webClient2 ] |> ResizeArray<Client>
