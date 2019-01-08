using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Quickstart;
using IdentityServer4.Test;

namespace IdentityServer4
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Api1",
                    "DomDaniel InCloud OrleansCluster Api",
                    new[] {JwtClaimTypes.Email, JwtClaimTypes.Role})
                {
                    ApiSecrets = new List<Secret> {new Secret(@"C%#4>#2x-kH(d9TuKqs?3Wt@NLT.\x$[".Sha256())}
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "ApiClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("ClientSuperSecret".Sha256())
                    },
                    Claims = new List<Claim> {new Claim(JwtClaimTypes.Role, "Admin")},
                    AllowedScopes =
                    {
                        "Api1",
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Role
                    }
                },
                new Client
                {
                    ClientId = "WebClient",
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret(@"~.M;H(CA,RDr6};A7F}#;Nfxg}A3m+kS".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "Api1"
                    },
                    RedirectUris = {"https://localhost:5004/signin-oidc"}
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return TestUsers.Users;
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}