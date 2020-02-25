namespace Orleans.Security.IntegrationTests.FSharp

open IdentityModel
open IdentityServer4.Models
open IdentityServer4.Services
open Orleans.Security.IntegrationTests.Grains.ClaimsBasedAuthorization
open Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
open System.Threading.Tasks
open System.Linq

type ProfileService() =
    interface IProfileService with
        member this.GetProfileDataAsync(context: ProfileDataRequestContext) =
            async {
                // Include DocRegistryAccessClaim
                let docRegistryAccessClaim =
                    context.Subject.Claims.SingleOrDefault(fun c -> c.Type = DocRegistryAccessClaim.Name)

                match docRegistryAccessClaim with
                | null -> ()
                | value -> context.IssuedClaims.Add value
                
                // Include Role claims
                let roleClaims =
                    context.Subject.Claims.Where(fun c -> c.Type = JwtClaimTypes.Role)
                    
                match roleClaims with
                | null -> ()
                | value -> context.IssuedClaims.AddRange value
                
                // Include CityClaim claims
                let cityClaims =
                    context.Subject.Claims.Where(fun c -> c.Type = CityClaim.Name)
                    
                match cityClaims with
                | null -> ()
                | value -> context.IssuedClaims.AddRange value
            }
            |> Async.StartAsTask :> Task

        member this.IsActiveAsync(context: IsActiveContext) = Task.CompletedTask
