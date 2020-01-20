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

type AccessTokenProvider(accessTokenResolver: unit -> string) =
    interface IAccessTokenProvider with
        member this.RetrieveTokenAsync() =
            let accessToken = accessTokenResolver()
            Task.FromResult(accessToken);

let getClusterClient (accessTokenResolver: unit -> string) =
    SiloHost.startSilo() |> ignore
    
    let accessTokenProvider =
        AccessTokenProvider(accessTokenResolver) :> IAccessTokenProvider
                                      
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