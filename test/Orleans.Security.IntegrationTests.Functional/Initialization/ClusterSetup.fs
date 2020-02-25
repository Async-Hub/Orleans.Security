module ClusterSetup

open Orleans
open Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization

let initDocumentsRegistry (getClusterClient: string -> IClusterClient) =
    let getToken =
        async {
            let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async "Alice" "Pass123$"
                                           "Api1 Orleans" |> Async.AwaitTask
            return accessTokenResponse.AccessToken }

    let accessToken = getToken |> Async.RunSynchronously
    let client = getClusterClient accessToken
    
    let document1 = Document()
    document1.Author <- Users.aliceId
    document1.Content <- "Some content 1."
    document1.Name <- "Document1"
    
    let document2 = Document()
    document2.Author <- Users.bobId
    document2.Content <- "Some content 2."
    document2.Name <- "Document2"
    
    client.GetGrain<IDocumentsRegistryGrain>(DocumentsRegistry.Default).Add document1 |> ignore
    client.GetGrain<IDocumentsRegistryGrain>(DocumentsRegistry.Default).Add document2 |> ignore
