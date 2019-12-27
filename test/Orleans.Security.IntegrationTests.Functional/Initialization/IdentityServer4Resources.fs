module IdentityServer4Resources

open IdentityModel
open IdentityServer4.Models
open System.Collections.Generic

let getApiResources() =
    let api1 = ApiResource "Api1"
    api1.UserClaims.Add JwtClaimTypes.Email
    api1.UserClaims.Add JwtClaimTypes.Role

    Secret(HashExtensions.Sha256 "Secret") |> api1.ApiSecrets.Add

    [api1] |> ResizeArray<ApiResource>

let getIdentityResources() =
    let resources = List<IdentityResource>()

    resources.Add(IdentityResources.Email())
    resources.Add(IdentityResources.Profile())
    resources.Add(IdentityResources.OpenId())

    resources
