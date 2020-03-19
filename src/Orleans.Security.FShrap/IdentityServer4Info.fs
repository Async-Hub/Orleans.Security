namespace Orleans.Security

type IdentityServer4Info(url: string, clientId: string, clientSecret: string, allowedScope: string,
                         discoveryEndpointEndpointPath: string) =
    let _discoveryEndpointEndpointPath =
        if System.String.IsNullOrWhiteSpace discoveryEndpointEndpointPath
        then ".well-known/openid-configuration"
        else discoveryEndpointEndpointPath

    member this.Url = url
    member this.ClientId = clientId
    member this.ClientSecret = clientSecret
    member this.AllowedScope = allowedScope
    member this.DiscoveryEndpointUrl = discoveryEndpointEndpointPath
