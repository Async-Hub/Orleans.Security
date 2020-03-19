namespace Orleans.Security.AccessToken

open System.IdentityModel.Tokens.Jwt

module AccessTokenAnalyzer =
    let isAccessTokenJwtToken (accessToken: string) =
        let handler = JwtSecurityTokenHandler()
        handler.CanReadToken accessToken

    let getTokenType accessToken =
        if isAccessTokenJwtToken accessToken
        then AccessTokenType.Jwt else AccessTokenType.Reference
