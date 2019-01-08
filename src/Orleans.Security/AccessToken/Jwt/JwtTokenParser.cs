using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using JWT;
using JWT.Serializers;

// ReSharper disable UnusedMember.Global

namespace Orleans.Security.AccessToken.Jwt
{
    public class JwtTokenParser
    {
        public string ExtractIssuer(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

            return jwtSecurityToken.Claims.First().Value;
        }

        public bool IsAccessTokenJwt(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();

            return handler.CanReadToken(accessToken);
        }

        public IEnumerable<Claim> ExtractClaims(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

            return jwtSecurityToken.Claims;
        }

        public string JwtToJson(string jwt)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            return decoder.Decode(jwt);
        }
    }
}
