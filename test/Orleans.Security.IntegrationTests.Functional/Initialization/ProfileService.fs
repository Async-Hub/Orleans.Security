namespace Orleans.Security.IntegrationTests.FSharp

open IdentityModel
open IdentityServer4.Models
open IdentityServer4.Services
open Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
open System.Threading.Tasks
open System.Linq

type ProfileService() =
    interface IProfileService with
        member this.GetProfileDataAsync(context: ProfileDataRequestContext) =
            async {
                let docRegistryAccessClaim =
                    context.Subject.Claims.SingleOrDefault(fun c -> c.Type = DocRegistryAccessClaim.Name)

                match docRegistryAccessClaim with
                | null -> ()
                | value -> context.IssuedClaims.Add value
                
                let roleClaims =
                    context.Subject.Claims.Where(fun c -> c.Type = JwtClaimTypes.Role)
                    
                match roleClaims with
                | null -> ()
                | value -> context.IssuedClaims.AddRange value
            }
            |> Async.StartAsTask :> Task

        member this.IsActiveAsync(context: IsActiveContext) = async { return 1 } |> Async.StartAsTask :> Task
