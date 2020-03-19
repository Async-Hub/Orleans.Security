namespace Orleans.Security.AccessToken

open System.IdentityModel.Tokens.Jwt
open System.Security.Cryptography
open IdentityModel
open Microsoft.IdentityModel.Tokens

module JwtSecurityTokenVerifier =
    let verify (jwt:string) (audience:string) (discoveryDocument: DiscoveryDocumentShortInfo) =
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
        
        

