namespace Orleans.Security.IntegrationTests.FSharp

open IdentityServer4.Models
open IdentityServer4.Services
open System.Threading.Tasks
open System.Linq

type ProfileService() =
    interface IProfileService with
        member this.GetProfileDataAsync(context: ProfileDataRequestContext) =
            async {
                let claim =
                    context.Subject.Claims.SingleOrDefault(fun c -> c.Type = GlobalConfig.defaultDocRegistryName)

                match claim with
                | null -> ()
                | value -> context.IssuedClaims.Add value
            }
            |> Async.StartAsTask :> Task

        member this.IsActiveAsync(context: IsActiveContext) = async { return 1 } |> Async.StartAsTask :> Task
