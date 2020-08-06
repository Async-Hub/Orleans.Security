namespace Orleans.Security

open System.Runtime.InteropServices

module Path=
    [<Literal>]
    let Url = ".well-known/openid-configuration"

type IdentityServer4Info(url: string, clientId: string, clientSecret: string, allowedScope: string,
                         [<Optional; DefaultParameterValue(Path.Url)>]discoveryEndpointEndpointPath: string)=
    member this.Url = url
    member this.ClientId = clientId
    member this.ClientSecret = clientSecret
    member this.AllowedScope = allowedScope
    member this.DiscoveryEndpointUrl = discoveryEndpointEndpointPath

//namespace Orleans.Security
//{
//    public class IdentityServer4Info
//    {
//        public IdentityServer4Info(string url,
//            string clientId, string clientSecret, string allowedScope,
//            string discoveryEndpointEndpointPath = ".well-known/openid-configuration")
//        {
//            Url = url;
//            ClientId = clientId;
//            ClientSecret = clientSecret;
//            AllowedScope = allowedScope;
//            DiscoveryEndpointUrl = Url + "/" + discoveryEndpointEndpointPath;
//        }

//        public string Url { get; }

//        public string ClientId { get; }

//        public string ClientSecret { get; }

//        public string AllowedScope { get; }

//        public string DiscoveryEndpointUrl { get; }
//    }
//}