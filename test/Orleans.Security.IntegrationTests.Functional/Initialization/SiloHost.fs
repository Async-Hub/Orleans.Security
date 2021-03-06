﻿module SiloHost

open Microsoft.Extensions.Hosting               
open Orleans.Configuration;
open Orleans.Hosting;
open Orleans.Security.Authorization
open Orleans.Security.Clustering;
open Orleans.Security.IntegrationTests.Grains
open Orleans.Security.IntegrationTests.Grains.SimpleAuthorization
open Orleans.Security;
open Orleans;
open System
open System.Net

let startSilo () =
    let builder = 
        HostBuilder()
            .UseEnvironment(Environments.Development)
            .UseOrleans(fun context siloBuilder ->          
               siloBuilder         
                   .UseLocalhostClustering()         
                   .Configure<ClusterOptions>(fun (options:ClusterOptions) ->         
                       options.ClusterId <- "Orleans.Security.Test"         
                       options.ServiceId <- "ServiceId"         
                       ignore())         
                   .Configure<EndpointOptions>(fun (options:EndpointOptions) ->          
                       options.AdvertisedIPAddress <- IPAddress.Loopback         
                       ignore())         
                   .ConfigureApplicationParts(fun parts ->          
                       parts.AddApplicationPart(typeof<SimpleGrain>.Assembly).WithReferences()         
                       |> ignore)
                   .AddMemoryGrainStorage("MemoryGrainStorage")
                   .ConfigureServices(fun services ->         
                       services.AddOrleansClusteringAuthorization(GlobalConfig.identityServer4Info,         
                           fun (config:Configuration) ->         
                           config.ConfigureAuthorizationOptions <- Action<AuthorizationOptions>(fun options ->         
                               AuthorizationConfig.ConfigureOptions(options) |> ignore)         
                           ignore())
                       // Some custom authorization services.
                       AuthorizationConfig.ConfigureServices(services)
                       ignore()) |> ignore         
               )         

    let host  = builder.Build()
    host.StartAsync().Wait()
    host



    
    