namespace Orleans.Security.AccessToken

open System.IdentityModel.Tokens.Jwt
open System.Security.Cryptography
open IdentityModel
open Microsoft.IdentityModel.Tokens

module JwtSecurityTokenVerifier =
    let Verify (jwt:string) (audience:string) (discoveryDocument: DiscoveryDocumentShortInfo) =
        let keys = System.Collections.Generic.List<SecurityKey>()
        for webKey in discoveryDocument.Keys do
            let e = Base64Url.Decode(webKey.E)
            let n = Base64Url.Decode(webKey.N)
            let rsaParameters = new RSAParameters(Exponent = e, Modulus = n)
            let key = RsaSecurityKey(rsaParameters)
            keys.Add key
        let parameters = TokenValidationParameters()
        parameters.ValidIssuer <- discoveryDocument.Issuer
        parameters.ValidAudience <- audience
        parameters.IssuerSigningKeys <- keys
        parameters.NameClaimType <- JwtClaimTypes.Name
        parameters.RoleClaimType <- JwtClaimTypes.Role
        parameters.RequireSignedTokens <- true
        
        let handler = JwtSecurityTokenHandler()
        handler.InboundClaimTypeMap.Clear()
        let mutable validatedToken : SecurityToken = null
        let claimsPrincipal = handler.ValidateToken(jwt, parameters, &validatedToken)
        claimsPrincipal.Claims
        
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using IdentityModel;
//using Microsoft.IdentityModel.Tokens;

//namespace Orleans.Security.AccessToken
//{
//    internal class JwtSecurityTokenVerifier
//    {
//        internal static IEnumerable<Claim> Verify(string jwt, string audience, 
//            DiscoveryDocumentShortInfo discoveryDocument)
//        {
//            var keys = new List<SecurityKey>();

//            foreach (var webKey in discoveryDocument.Keys)
//            {
//                var e = Base64Url.Decode(webKey.E);
//                var n = Base64Url.Decode(webKey.N);

//                var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
//                {
//                    KeyId = webKey.Kid
//                };

//                keys.Add(key);
//            }

//            var parameters = new TokenValidationParameters
//            {
//                ValidIssuer = discoveryDocument.Issuer,
//                ValidAudience = audience,
//                IssuerSigningKeys = keys,

//                NameClaimType = JwtClaimTypes.Name,
//                RoleClaimType = JwtClaimTypes.Role,

//                RequireSignedTokens = true
//            };

//            var handler = new JwtSecurityTokenHandler();
//            handler.InboundClaimTypeMap.Clear();

//            var claimsPrincipal = handler.ValidateToken(jwt, parameters, out var validatedToken);
            
//            return claimsPrincipal.Claims;
//        }
//    }
//}      

