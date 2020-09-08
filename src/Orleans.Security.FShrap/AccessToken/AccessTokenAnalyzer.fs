namespace Orleans.Security.AccessToken

open System.IdentityModel.Tokens.Jwt

module AccessTokenAnalyzer =
    let isAccessTokenJwtToken (accessToken: string) =
        let handler = JwtSecurityTokenHandler()
        handler.CanReadToken accessToken

    let GetTokenType accessToken =
        if isAccessTokenJwtToken accessToken
        then AccessTokenType.Jwt else AccessTokenType.Reference

//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using JWT;
//using JWT.Serializers;

//namespace Orleans.Security.AccessToken
//{
//    internal class AccessTokenAnalyzer
//    {
//        internal static AccessTokenType GetTokenType(string accessToken)
//        {
//            // ReSharper disable once ConvertIfStatementToReturnStatement
//            if (IsAccessTokenJwtToken(accessToken))
//            {
//                return AccessTokenType.Jwt;
//            }

//            return AccessTokenType.Reference;
//        }

//        public string ExtractIssuer(string jwtToken)
//        {
//            var handler = new JwtSecurityTokenHandler();
//            var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

//            return jwtSecurityToken.Claims.First().Value;
//        }

//        public IEnumerable<Claim> ExtractClaims(string jwtToken)
//        {
//            var handler = new JwtSecurityTokenHandler();
//            var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

//            return jwtSecurityToken.Claims;
//        }

//        public string JwtToJson(string jwt)
//        {
//            IJsonSerializer serializer = new JsonNetSerializer();
//            IDateTimeProvider provider = new UtcDateTimeProvider();
//            IJwtValidator validator = new JwtValidator(serializer, provider);
//            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
//            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

//            return decoder.Decode(jwt);
//        }

//        private static bool IsAccessTokenJwtToken(string accessToken)
//        {
//            var handler = new JwtSecurityTokenHandler();

//            return handler.CanReadToken(accessToken);
//        }
//    }
//}