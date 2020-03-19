namespace Orleans.Security.AccessToken

open System.Threading.Tasks

type internal IAccessTokenIntrospectionService =
    abstract IntrospectTokenAsync: accessToken:string
     -> allowOfflineValidation:bool -> Task<AccessTokenIntrospectionResult>