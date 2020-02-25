module IdSTokenFactory

open IdentityModel.Client
open IdSInstance

let getAccessTokenForClientAsync (clientId: string) (clientSecret: string) (scope: string) =
    let tokenRequest =
        new ClientCredentialsTokenRequest(Address = discoveryDocument.TokenEndpoint,
                                          Scope = scope, ClientId = clientId,
                                          ClientSecret = clientSecret)

    client.RequestClientCredentialsTokenAsync(tokenRequest)
    
let getAccessTokenForUserAsync (clientId: string) (clientSecret: string) (userName: string)
    (password: string) (scope: string) =
    let passwordTokenRequest = new PasswordTokenRequest(
                                Address = discoveryDocument.TokenEndpoint,
                                ClientId = clientId,
                                ClientSecret = clientSecret,
                                UserName = userName,
                                Password = password,
                                Scope = scope)

    client.RequestPasswordTokenAsync(passwordTokenRequest)

let getAccessTokenForUserOnWebClient1Async = getAccessTokenForUserAsync GlobalConfig.WebClient1 "Secret1"
let getAccessTokenForUserOnWebClient2Async = getAccessTokenForUserAsync GlobalConfig.WebClient2 "Secret2"

