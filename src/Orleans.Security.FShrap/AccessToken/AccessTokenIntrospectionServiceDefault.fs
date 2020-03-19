namespace Orleans.Security.AccessToken

open System.Net.Http
open IdentityModel.Client
open Microsoft.Extensions.Logging
open Orleans.Security

type internal AccessTokenIntrospectionServiceDefault(httpClientFactory: IHttpClientFactory,
                                                     identityServer4Info : IdentityServer4Info,
                                                     discoveryDocumentProvider:IdS4DiscoveryDocumentProvider,
                                                     logger: ILogger<AccessTokenIntrospectionServiceDefault> 
                                                    )=
    let httpClient = httpClientFactory.CreateClient "IdS4"
    
    
    member private this.IntrospectTokenOnlineAsync (accessToken:string) (accessTokenType: AccessTokenType)
            (discoveryDocument:DiscoveryDocumentShortInfo) =
            async{
                let request = TokenIntrospectionRequest()
                request.Address <- discoveryDocument.IntrospectionEndpoint
                request.ClientId <- identityServer4Info.ClientId
                request.Token <- accessToken
                request.ClientSecret <- identityServer4Info.ClientSecret
                
                let! introspectionResponse = httpClient.IntrospectTokenAsync request |> Async.AwaitTask
                let nameOfTokenType =
                    match accessTokenType with
                    | AccessTokenType.Jwt -> "JWT"
                    |_-> "Reference"
                
                if not introspectionResponse.IsError then
                    return AccessTokenIntrospectionResult(accessTokenType, introspectionResponse.Claims,
                                                          introspectionResponse.IsActive, "")
                else
                    // TODO: Log trace.
                    return AccessTokenIntrospectionResult(accessTokenType, introspectionResponse.Claims,
                                                          false, "")
            } |> Async.StartAsTask
    
    interface IAccessTokenIntrospectionService with
        member this.IntrospectTokenAsync accessToken allowOfflineValidation =
          async{
              let accessTokenType = AccessTokenAnalyzer.getTokenType accessToken
              let! discoveryResponse = discoveryDocumentProvider.GetDiscoveryDocumentAsync() |> Async.AwaitTask
              
              if accessTokenType = AccessTokenType.Jwt && allowOfflineValidation then
                  let claims = JwtSecurityTokenVerifier.verify accessToken
                                   identityServer4Info.AllowedScope discoveryResponse
                  return AccessTokenIntrospectionResult(accessTokenType, claims, true, "")
              else
                  let! res = this.IntrospectTokenOnlineAsync accessToken accessTokenType discoveryResponse |> Async.AwaitTask
                  return res
          } |> Async.StartAsTask
        end
        
