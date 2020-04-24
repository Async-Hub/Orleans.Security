namespace Orleans.Security.AccessToken

open System.Threading.Tasks

type IAccessTokenVerifier =
    abstract Verify: accessToken : string -> Task<AccessTokenVerificationResult>
