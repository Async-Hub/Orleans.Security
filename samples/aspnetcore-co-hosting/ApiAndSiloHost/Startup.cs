using IdentityModel.AspNetCore.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Orleans.Security;

namespace ApiAndSiloHost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var oAuth2EndpointInfo = new IdentityServer4Info("http://localhost:5000",
                "Api1", @"TFGB=?Gf3UvH+Uqfu_5p", "Orleans");

            services.AddAuthentication("token")
                // JWT tokens
                .AddJwtBearer("token", options =>
                {
                    // For development environments only. Do not use for production.
                    options.RequireHttpsMetadata = false;

                    options.Authority = oAuth2EndpointInfo.Url;
                    options.Audience = "Api1";
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                    // if token does not contain a dot, it is a reference token
                    // https://leastprivilege.com/2020/07/06/flexible-access-token-validation-in-asp-net-core/
                    options.ForwardDefaultSelector = Selector.ForwardReferenceToken("introspection");
                })

                // reference tokens
                .AddOAuth2Introspection("introspection", options =>
                {
                    options.Authority = oAuth2EndpointInfo.Url;

                    options.ClientId = oAuth2EndpointInfo.ClientId;
                    options.ClientSecret = oAuth2EndpointInfo.ClientSecret;
                });

            services.AddControllers();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
