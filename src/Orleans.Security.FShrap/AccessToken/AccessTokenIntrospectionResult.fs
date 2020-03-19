namespace Orleans.Security.AccessToken

open System.Collections.Generic
open System.Security.Claims

type AccessTokenIntrospectionResult(accessTokenType: AccessTokenType,
                                    claims: IEnumerable<Claim>,
                                    isValid: bool, message: string) =
    member this.AccessTokenType = accessTokenType
    member this.Claims = claims
    member this.IsValid = isValid
    member this.Message = message