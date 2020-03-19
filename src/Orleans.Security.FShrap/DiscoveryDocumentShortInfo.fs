namespace Orleans.Security.AccessToken

open IdentityModel.Jwk
open System.Collections.Generic

[<AllowNullLiteral>]
type DiscoveryDocumentShortInfo() =
    member val IntrospectionEndpoint : string = null with get, set
    
    member val Issuer : string = null with get, set
    
    member val Keys : IList<JsonWebKey> = null with get,set

    member val TokenEndpoint : string = null with get,set
