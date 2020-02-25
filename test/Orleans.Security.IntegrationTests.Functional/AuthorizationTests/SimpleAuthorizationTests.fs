module SimpleAuthorizationTests

open System
open FluentAssertions
open Orleans.Security
open Orleans.Security.IntegrationTests.Grains
open Orleans.Security.IntegrationTests.Grains.SimpleAuthorization
open System.Threading.Tasks
open Xunit

[<Theory>]
[<InlineData("Bob", "Pass123$", "Api1 Orleans")>]
let ``An authenticated user can invoke the grain method``
    (userName: string) (password: string) (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async
                                       userName password scope |> Async.AwaitTask

        let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
        let simpleGrain = clusterClient.GetGrain<ISimpleGrain>(Guid.NewGuid())
        let! value = simpleGrain.GetWithAuthenticatedUser("Secret") |> Async.AwaitTask

        Assert.True(value.Equals "Secret")
    }
    
[<Theory>]
[<InlineData("Alice", "Pass123$", "Api1")>]
let ``An authenticated user on an unauthenticated client can't invoke the grain method``
    (userName: string) (password: string) (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient2Async
                                       userName password scope |> Async.AwaitTask

        let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
        let simpleGrain = clusterClient.GetGrain<ISimpleGrain>(Guid.NewGuid())
        
        // Act
        let action =
            async{
                let! value = simpleGrain.GetValue() |> Async.AwaitTask
            return value } |> Async.StartAsTask :> Task

        Assert.ThrowsAsync<OrleansClusterUnauthorizedAccessException>(fun () -> action) |> ignore
    }
    
[<Fact>]
let ``An anonymous user can't invoke the grain method`` () =
    async {
        // Arrange
        let accessToken = String.Empty
        let userName = "Empty"
        
        let clusterClient = SiloClient.getClusterClient accessToken
        let simpleGrain = clusterClient.GetGrain<ISimpleGrain>(Guid.NewGuid())
        
        // Act
        let action =
            async {
                let! value = simpleGrain.GetWithAuthenticatedUser(String.Empty) |> Async.AwaitTask
                return value } |> Async.StartAsTask :> Task

        Assert.ThrowsAsync<OrleansClusterUnauthorizedAccessException>(fun () -> action) |> ignore
    }

[<Fact>]
let ``An anonymous user can invoke a grain method with [AllowAnonymous] attribyte`` () =
    async {
        // Arrange
        let accessToken = String.Empty

        let clusterClient = SiloClient.getClusterClient accessToken
        let simpleGrain = clusterClient.GetGrain<ISimpleGrain>(Guid.Empty)

        let! value = simpleGrain.GetWithAnonymousUser("Secret") |> Async.AwaitTask

        Assert.True(value.Equals "Secret")
    }


