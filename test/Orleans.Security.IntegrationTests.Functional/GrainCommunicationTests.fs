module GrainCommunicationTests

open Orleans.Security.IntegrationTests.Grains
open Xunit

[<Theory>]
[<InlineData("Alice", "Pass123$", "Api1 Orleans")>]
let ``Test``
    (userName: string) (password: string) (scope: string) =
        async {
            // Arrange
            let! accessTokenResponse =
                IdentityServer4Client.getAccessTokenWAsync userName password scope
                |> Async.AwaitTask
                 
            let clusterClient = SiloClient.getClusterClient(fun () -> accessTokenResponse.AccessToken);
            let userGrain = clusterClient.GetGrain<IUserGrain>("Alice")
            let! value = userGrain.TakeSecret() |> Async.AwaitTask
                                       
            Assert.True(value.Equals "Alice super secret data...")
        }