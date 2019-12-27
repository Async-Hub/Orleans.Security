module AccessTokenVerificationTests

open Xunit
open Orleans.Security.AccessToken

[<Theory>]
[<InlineData("WebClient", "Secret", "Api1")>]
let ``Access token verification with valid scope should be passed`` 
    (clientId:string) (clientSecret:string) (scope:string) = async {
        // Arrange
        let! accessTokenResponse = IdentityServer4.requestClientCredentialsTokenAsync 
                                    clientId clientSecret scope |> Async.AwaitTask

        // Act
        let claims = JwtSecurityTokenVerifier.Verify(accessTokenResponse.AccessToken, 
                        scope, IdentityServer4.discoveryDocument)
    
        // Assert
        Assert.True(claims <> null)
    }