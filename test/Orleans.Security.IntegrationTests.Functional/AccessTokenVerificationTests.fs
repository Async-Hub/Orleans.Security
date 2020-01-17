module AccessTokenVerificationTests

open Xunit
open Microsoft.IdentityModel.Tokens
open Orleans.Security.AccessToken

[<Theory>]
[<InlineData("WebClient", "Secret", "Api1")>]
let ``Access token verification with valid scope should be passed``
    (clientId: string) (clientSecret: string) (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdentityServer4.requestClientCredentialsTokenAsync clientId clientSecret scope
                                   |> Async.AwaitTask

        // Act
        let claims =
            JwtSecurityTokenVerifier.Verify(accessTokenResponse.AccessToken, scope, IdentityServer4.discoveryDocument)

        // Assert
        Assert.True(claims |> Seq.exists (fun c -> c.Type = "aud" && c.Value = scope))
    }

[<Theory>]
[<InlineData("WebClient", "Secret", "Api2")>]
let ``Access token verification with invalid scope should be failed``
    (clientId: string) (clientSecret: string) (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdentityServer4.requestClientCredentialsTokenAsync clientId clientSecret "Api1"
                                   |> Async.AwaitTask

        // Act
        let verify =
            fun () ->
                JwtSecurityTokenVerifier.Verify
                    (accessTokenResponse.AccessToken, scope, IdentityServer4.discoveryDocument) |> ignore

        // Assert
        Assert.Throws<SecurityTokenInvalidAudienceException>(verify) |> ignore
    }
