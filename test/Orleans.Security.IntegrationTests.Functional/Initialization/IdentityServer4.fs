module IdentityServer4

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

type Startup() =
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddIdentityServer().AddDeveloperSigningCredential()
                .AddInMemoryApiResources(IdentityServer4Resources.getApiResources())
                .AddInMemoryIdentityResources(IdentityServer4Resources.getIdentityResources())
                .AddInMemoryClients(Clients.getClients()).AddTestUsers(Users.getUsers()) |> ignore

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


