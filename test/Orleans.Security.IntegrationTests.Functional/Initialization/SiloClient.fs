module SiloClient

open Microsoft.Extensions.DependencyInjection
open Orleans.Configuration;
open Orleans.Security.Authorization
open Orleans.Security.Client
open Orleans.Security.IntegrationTests.Grains
open Orleans.Security;
open Orleans;
open System
open System.Threading.Tasks

type AccessTokenProvider() =
    let mutable accessToken = String.Empty
    member this.AccessToken
            with set (value) = accessToken <- value
    
    interface IAccessTokenProvider with
        member this.RetrieveTokenAsync() =
            Task.FromResult(accessToken);

let private globalAccessToken = AccessTokenProvider()
let accessTokenProvider = globalAccessToken :> IAccessTokenProvider

let private clusterClient =
    SiloHost.startSilo() |> ignore
                         
    let builder = ClientBuilder()
                    .UseLocalhostClustering()
                    .Configure<ClusterOptions>(fun (options: ClusterOptions) ->
                        options.ClusterId <- "Orleans.Security.Test"
                        options.ServiceId <- "ServiceId"
                        ignore())
                    .ConfigureApplicationParts(fun parts -> 
                                  parts.AddApplicationPart(typeof<UserGrain>.Assembly).WithReferences() |> ignore)
                    .ConfigureServices(fun services ->
                                  services.AddOrleansClusteringAuthorization(GlobalConfig.identityServer4Info,
                                      fun (config:Configuration) ->
                                      config.ConfigureAuthorizationOptions <- Action<AuthorizationOptions>(fun options ->
                                          AuthorizationConfig.ConfigureOptions(options) |> ignore)
                                      ignore())
                                  services.AddSingleton<IAccessTokenProvider>( fun _ -> accessTokenProvider)
                                  |> ignore)

    let clusterClient = builder.Build()
    clusterClient.Connect().Wait()
    clusterClient
    
let getClusterClient (accessToken: string) =
    globalAccessToken.AccessToken <- accessToken
    clusterClient