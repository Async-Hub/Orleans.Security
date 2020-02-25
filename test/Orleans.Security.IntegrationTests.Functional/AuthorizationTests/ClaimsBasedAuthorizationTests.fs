module ClaimsBasedAuthorizationTests

open FluentAssertions
open Orleans.Security
open Orleans.Security.IntegrationTests.Grains.ClaimsBasedAuthorization
open Orleans.Security.IntegrationTests.Grains.PolicyBasedAuthorization
open System
open System.Threading.Tasks
open Xunit

[<Theory>]
[<InlineData("Alice", "Pass123$", "Api1 Orleans")>]
let ``A user with an appropriate claim should have access to the method`` (userName: string) (password: string)
    (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async userName password scope
                                   |> Async.AwaitTask

        // Act
        let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
        let claimGrain = clusterClient.GetGrain<IClaimGrain>(userName)
        let! value = claimGrain.DoSomething("Secret") |> Async.AwaitTask

        Assert.True(value.Equals "Secret")
    }

[<Theory>]
[<InlineData("Bob", "Pass123$", "Api1 Orleans")>]
let ``A user without an appropriate claim shouldn't have access to the method`` (userName: string) (password: string)
    (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async userName password scope
                                   |> Async.AwaitTask

        let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
        let userGrain = clusterClient.GetGrain<IPolicyGrain>(userName)

        // Act
        let action =
            async {
                let! value = userGrain.GetWithMangerPolicy(String.Empty) |> Async.AwaitTask
                return value } |> Async.StartAsTask :> Task

        Assert.ThrowsAsync<OrleansClusterUnauthorizedAccessException>(fun () -> action) |> ignore
    }

[<Theory>]
[<InlineData("Carol", "Pass123$", "Api1 Orleans")>]
let ``A user with an appropriate claim and without an appropriate claim value shouldn't have access to the method``
    (userName: string) (password: string) (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async userName password scope
                                   |> Async.AwaitTask

        // Act
        let action =
            async {
                let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
                let claimGrain = clusterClient.GetGrain<IClaimGrain>(userName)
                let! value = claimGrain.DoSomething("Secret") |> Async.AwaitTask
                return value
            } |> Async.StartAsTask :> Task


        Assert.ThrowsAsync<OrleansClusterUnauthorizedAccessException>(fun () -> action) |> ignore
    }
