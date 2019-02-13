using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;

namespace Orleans.Security.AccessToken
{
    internal class JwtVerifier
    {
        internal static IEnumerable<Claim> Verify(string jwt, string audience, DiscoveryResponse discoveryDocument)
        {
            var keys = new List<SecurityKey>();

            foreach (var webKey in discoveryDocument.KeySet.Keys)
            {
                var e = Base64Url.Decode(webKey.E);
                var n = Base64Url.Decode(webKey.N);

                var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
                {
                    KeyId = webKey.Kid
                };

                keys.Add(key);
            }

            var parameters = new TokenValidationParameters
            {
                ValidIssuer = discoveryDocument.Issuer,
                ValidAudience = audience,
                IssuerSigningKeys = keys,

                NameClaimType = JwtClaimTypes.Name,
                RoleClaimType = JwtClaimTypes.Role,

                RequireSignedTokens = true
            };

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();

            var claimsPrincipal = handler.ValidateToken(jwt, parameters, out var validatedToken);
            
            return claimsPrincipal.Claims;
        }
    }
}
