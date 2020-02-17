module ClusterSetup

open IdentityModel.Client
open Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization

let initDocumentsRegistry =
    let getToken =
        async {
            let! accessTokenResponse = IdSTokenFactory.getAccessTokenForUserOnWebClient1Async "Alice" "Pass123$"
                                           "Api1 Orleans" |> Async.AwaitTask
            return accessTokenResponse.AccessToken }

    let accessToken = getToken |> Async.RunSynchronously
    let client = SiloClient.getClusterClient accessToken
    
    let document1 = Document()
    document1.Author <- Users.aliceEmail
    document1.Content <- "Some content 1."
    document1.Name <- "Document1"
    
    let document2 = Document()
    document2.Author <- Users.bobEmail
    document2.Content <- "Some content 2."
    document2.Name <- "Document2"
    
    client.GetGrain<IDocumentsRegistryGrain>("defaultDocRegistryName").Add document1 |> ignore
    client.GetGrain<IDocumentsRegistryGrain>("defaultDocRegistryName").Add document2 |> ignore
    
do initDocumentsRegistry
