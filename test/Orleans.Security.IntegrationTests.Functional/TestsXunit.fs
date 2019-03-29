module TestsXunit

open Xunit
open Configuration
open Orleans.Security.AccessToken

[<Theory>]
[<InlineData("WebClient", "Secret", "Api1")>]
let ``Access token verification with valid scope should be passed`` 
    (clientId:string) (clientSecret:string) (scope:string) = async {
        // Arrange
        let! accessTokenResponse = IdentityServer4Builder.requestClientCredentialsTokenAsync 
                                    clientId clientSecret scope |> Async.AwaitTask

        // Act
        let claims = JwtSecurityTokenVerifier.Verify(accessTokenResponse.AccessToken, 
                        scope, IdentityServer4Builder.discoveryDocument)
    
        // Assert
        Assert.True(claims <> null)
    }