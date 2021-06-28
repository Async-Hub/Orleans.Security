namespace Orleans.Security

open System
open Orleans.Security.AccessToken
open Orleans.Security.Authorization

type Configuration()=
    member val ConfigureAccessTokenVerifierOptions : Action<AccessTokenVerifierOptions> = null with get,set
    member val ConfigureAuthorizationOptions : Action<AuthorizationOptions> = null with get,set
    member val ConfigureSecurityOptions: Action<SecurityOptions> = null with get,set
    member val TracingEnabled = false with get, set

//using System;
//using Orleans.Security.AccessToken;
//using Orleans.Security.Authorization;

//namespace Orleans.Security
//{
//    public class Configuration
//    {
//        public Action<AccessTokenVerifierOptions> ConfigureAccessTokenVerifierOptions { get; set; }

//        public Action<AuthorizationOptions> ConfigureAuthorizationOptions { get; set; }

//        public bool TracingEnabled { get; set; }
//    }
//}