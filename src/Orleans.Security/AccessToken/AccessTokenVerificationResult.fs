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

//using System.Collections.Generic;
//using System.Security.Claims;

//namespace Orleans.Security.AccessToken
//{
//    public class AccessTokenVerificationResult
//    {
//        private AccessTokenVerificationResult(bool isVerified)
//        {
//            IsVerified = isVerified;
//        }

//        public IEnumerable<Claim> Claims { get; private set; }

//        public string InvalidValidationMessage { get; private set; }

//        public bool IsVerified { get; }

//        // ReSharper disable once UnusedAutoPropertyAccessor.Local
//        private AccessTokenType? TokenType { get; set; }

//        public static AccessTokenVerificationResult CreateFailed(string message)
//        {
//            return new AccessTokenVerificationResult(false)
//            {
//                InvalidValidationMessage = message
//            };
//        }

//        public static AccessTokenVerificationResult CreateSuccess(AccessTokenType accessTokenType,
//            IEnumerable<Claim> claims)
//        {
//            return new AccessTokenVerificationResult(true)
//            {
//                Claims = claims,
//                TokenType = accessTokenType
//            };
//        }
//    }
//}