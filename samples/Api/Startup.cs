using Api.Orleans;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using Orleans.Security;

namespace Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var oAuth2EndpointInfo = new IdentityServer4Info("https://localhost:5001",
                "Api1", @"C%#4>#2x-kH(d9TuKqs?3Wt@NLT.\x$[");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.SupportedTokens = SupportedTokens.Both;
                    options.Authority = oAuth2EndpointInfo.Url;
                    options.ApiName = oAuth2EndpointInfo.ClientScopeName;
                    options.ApiSecret = oAuth2EndpointInfo.ClientSecret;
                    options.SaveToken = true;
                });

            services.AddMvcCore().AddAuthorization().AddJsonFormatters();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ReSharper disable once RedundantTypeArgumentsOfMethod
            services.AddSingleton<IClusterClient>(serviceProvider =>
            {
                OrleansClusterClientProvider.StartClientWithRetries(out var client,
                    serviceProvider.GetService<IHttpContextAccessor>(), oAuth2EndpointInfo);

                return client;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
