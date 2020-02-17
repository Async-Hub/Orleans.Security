module ResourceBasedAuthorizationTests

open FluentAssertions
open Orleans.Security
open Orleans.Security.IntegrationTests.Grains
open Orleans.Security.IntegrationTests.Grains.PolicyBasedAuthorization
open System
open System.Threading.Tasks
open Xunit

[<Theory>]
[<InlineData("Alice", "Pass123$", "Api1 Orleans")>]
let ``A user with an appropriate permisiion can read document content``
    (userName: string) (password: string) (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async
                                       userName password scope |> Async.AwaitTask

        let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
        let userGrain = clusterClient.GetGrain<IUserGrain>(userName)
        let! value = userGrain.GetDocumentContent("Document1") |> Async.AwaitTask

        Assert.True(value.Equals "Some content 1.")
    }