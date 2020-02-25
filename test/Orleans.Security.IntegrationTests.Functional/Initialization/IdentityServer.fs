module IdentityServer

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open IdentityServer4.Services
open Orleans.Security.IntegrationTests.FSharp

type Startup() =
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddIdentityServer().AddDeveloperSigningCredential()
                .AddInMemoryApiResources(IdSResources.getApiResources())
                .AddInMemoryIdentityResources(IdSResources.getIdentityResources())
                .AddInMemoryClients(IdSClients.getClients()).AddTestUsers(Users.getUsers()) |> ignore

        services.AddTransient<IProfileService, ProfileService>() |> ignore
        services.AddControllersWithViews() |> ignore

    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        app.UseStaticFiles().UseIdentityServer().UseRouting()
           .UseEndpoints(fun endpoints -> endpoints.MapDefaultControllerRoute() |> ignore) |> ignore

let createHostBuilder =
    Host.CreateDefaultBuilder([||])
        .ConfigureWebHostDefaults(fun webBuilder ->
            webBuilder.UseUrls(GlobalConfig.identityServer4Url).UseStartup<Startup>() |> ignore)

let startServer() =
    createHostBuilder.Build().RunAsync()