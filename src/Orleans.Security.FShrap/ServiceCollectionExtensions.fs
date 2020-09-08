namespace Orleans.Security
open System.Runtime.CompilerServices
open System
open System.Net.Http
open Microsoft.Extensions.Caching.Memory
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.DependencyInjection.Extensions
open Microsoft.Extensions.Logging
open Orleans.Security.AccessToken
open Orleans.Security.Authorization
open Microsoft.FSharp.Core.Operators
//open Orleans.Security.Caching

//[<Extension>]
//type ServiceCollectionExtensions() =
//    [<Extension>]
//    static member AddOrleansClusterSecurityServices(services : IServiceCollection, 
//        configure: Action<Configuration>, configureServices : Action<IServiceCollection>) =
//            // TODO: Use 'nameof' for method params.
//            if(services = null) then raise(ArgumentNullException("services")) 
//            else
//                let configuration = new Configuration()
//                configure.Invoke configuration |> ignore
//                services.AddAuthorizationCore configuration.ConfigureAuthorizationOptions |> ignore
//                //services.TryAdd(ServiceDescriptor.Singleton<IAuthorizationExecutor, AuthorizationExecutor>())

//                services.TryAddSingleton<DefaultAccessTokenVerifier>();
//                services.AddTransient<IAccessTokenIntrospectionService, AccessTokenIntrospectionServiceDefault>();