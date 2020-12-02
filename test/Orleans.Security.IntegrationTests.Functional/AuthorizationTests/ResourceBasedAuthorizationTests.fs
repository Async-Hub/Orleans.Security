module ResourceBasedAuthorizationTests

open System.Threading.Tasks
open FluentAssertions
open Orleans.Security
open Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
open Xunit

[<Theory>]
[<InlineData("Bob", "Pass123$", "Api1 Orleans")>]
let ``A user with appropriate permission can read a document``
    (userName: string) (password: string) (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async
                                       userName password scope |> Async.AwaitTask

        let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
        let userGrain = clusterClient.GetGrain<IUserGrain>(userName)
        let! value = userGrain.GetDocumentContent("Document2") |> Async.AwaitTask

        Assert.True(value.Equals "Some content 2.")
    }
    
[<Theory>]
[<InlineData("Carol", "Pass123$", "Api1 Orleans")>]
let ``A user without appropriate permission can't read a document``
    (userName: string) (password: string) (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async
                                       userName password scope |> Async.AwaitTask

        let action =
            async{
                let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
                let userGrain = clusterClient.GetGrain<IUserGrain>(userName)
                let! value = userGrain.GetDocumentContent("Document1") |> Async.AwaitTask
                return value
            } |> Async.StartAsTask :> Task
        

        Assert.ThrowsAsync<NotAuthorizedException>(fun () -> action) |> ignore
    }
    
[<Theory>]
[<InlineData("Alice", "Pass123$", "Api1 Orleans")>]
let ``A user with appropriate permission can modify a document``
    (userName: string) (password: string) (scope: string) =
    async {
        // Arrange
        let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async
                                       userName password scope |> Async.AwaitTask

        let newContent = "Some new content!";
        let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
        let userGrain = clusterClient.GetGrain<IUserGrain>(userName)
        let! value = userGrain.ModifyDocumentContent("Document1", newContent) |> Async.AwaitTask

        Assert.True(value.Equals newContent)
    }
    
[<Theory>]
[<InlineData("Alice", "Pass123$", "Api1 Orleans")>]
let ``A user without appropriate permission can't modify a document``
    (userName: string) (password: string) (scope: string) =
    async {
        // Arrange
        // This is Bob's document.
        let documentName = "Document2";
        let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async
                                       userName password scope |> Async.AwaitTask

        let newContent = "Some new content!";
        let clusterClient = SiloClient.getClusterClient accessTokenResponse.AccessToken
        let userGrain = clusterClient.GetGrain<IUserGrain>(userName)
        let! value = userGrain.ModifyDocumentContent(documentName, newContent) |> Async.AwaitTask

        Assert.Null(value)
    }