module SimpleAuthorizationTests

open System
open FluentAssertions
open Orleans.Security
open Orleans.Security.IntegrationTests.Grains
open System.Threading.Tasks
open Xunit

[<Theory>]
[<InlineData("Bob", "Pass123$", "Api1 Orleans")>]
let ``An authenticated user can invoke the grain method`` (userName: string) (password: string)
    (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdentityServer4Client.getAccessTokenWAsync userName password scope |> Async.AwaitTask

        let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
        let userGrain = clusterClient.GetGrain<IUserGrain>(userName)
        let! value = userGrain.GetWithAuthenticatedUser("Secret") |> Async.AwaitTask

        Assert.True(value.Equals "Secret")
    }
    
[<Fact>]
let ``An anonymous user can't invoke the grain method`` () =
    async {
        // Arrange
        let accessToken = String.Empty
        let userName = "Empty"
        
        let clusterClient = SiloClient.getClusterClient accessToken
        let userGrain = clusterClient.GetGrain<IUserGrain>(userName)
        
        // Act
        let action =
            async {
                let! value = userGrain.GetWithAuthenticatedUser(String.Empty) |> Async.AwaitTask
                return value } |> Async.StartAsTask :> Task

        Assert.ThrowsAsync<OrleansClusterUnauthorizedAccessException>(fun () -> action) |> ignore
    }

[<Fact>]
let ``An anonymous user can invoke a grain method with [AllowAnonymous] attribyte`` () =
    async {
        // Arrange
        let accessToken = String.Empty
        let userName = "Empty"
        
        let clusterClient = SiloClient.getClusterClient accessToken
        let userGrain = clusterClient.GetGrain<IUserGrain>(userName)

        let! value = userGrain.GetWithAnonymousUser("Secret") |> Async.AwaitTask

        Assert.True(value.Equals "Secret")
    }


