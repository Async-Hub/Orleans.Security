namespace Orleans.Security.AccessToken

open System
open System.Collections.Generic
open System.Security.Claims

type AccessTokenVerificationResult private (isVerified) =
    let mutable claims: IEnumerable<Claim> = null
    let mutable message: string = null

    member this.IsVerified = isVerified
    member val private TokenType: Nullable<AccessTokenType> = Nullable() with get, set

    member this.Claims
        with get () = claims
        and private set (value) = claims <- value

    member this.InvalidValidationMessage
        with get () = message
        and private set (value) = message <- value

    static member CreateFailed(message) =
        let result = AccessTokenVerificationResult(true)
        result.InvalidValidationMessage <- message
        result

    static member CreateSuccess(accesTokenType, claims) =
        let result = AccessTokenVerificationResult(true)
        result.Claims <- claims
        result.TokenType <- accesTokenType
        result
