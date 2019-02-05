using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Security.AccessToken;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Configuration
{
    internal static class AuthorizationTestConfig
    {
        // internal const string AccessToken = "AccessToken";

        internal static readonly Dictionary<int, IEnumerable<Claim>> Claims =
            new Dictionary<int, IEnumerable<Claim>>
            {
                {
                    (int) LoggedInUser.AliceSmith,
                    new[]
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Role, "Admin"),
                        new Claim(JwtClaimTypes.Role, "Developer"),
                        new Claim(JwtClaimTypes.Role, "Manager"),
                        new Claim(JwtClaimTypes.Gender, "Female")
                    }
                },
                {
                    (int) LoggedInUser.BobSmith,
                    new[]
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Role, "Manager")
                    }
                }
            };

        internal static void ConfigureOptions(AuthorizationOptions options)
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
            options.AddPolicy("EmailVerifiedPolicy",
                policy => policy.Requirements.Add(new EmailVerifiedRequirement(true)));
            options.AddPolicy("FemalePolicy", policy => policy.RequireClaim(JwtClaimTypes.Gender, "Female"));
            options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));

            options.AddPolicy("FemaleAdminPolicy", policy =>
                {
                    policy.Requirements.Add(new RoleIsPresentRequirement("Admin"));
                    policy.Requirements.Add(new GenderRequirement("Female"));
                }
            );

            options.AddPolicy("FemaleManagerPolicy", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(claim => claim.Type == JwtClaimTypes.Gender &&
                                                              claim.Value == "Female");
                    });
                }
            );
        }

        internal static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAccessTokenVerifier, FakeAccessTokenVerifier>();
            services.AddSingleton<IAuthorizationHandler, EmailVerifiedHandler>();
            services.AddSingleton<IAuthorizationHandler, RoleAndGenderCombinationHandler>();
        }
    }
}