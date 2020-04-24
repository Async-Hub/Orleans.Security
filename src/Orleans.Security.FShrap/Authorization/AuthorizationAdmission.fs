namespace Orleans.Security.Authorization

open Orleans
open System.Reflection
open System.Collections.Generic
open System.Linq

type internal AuthorizationAdmission() =
    static member IsRequired(grainCallContext: IGrainCallContext) =
        let allowAnonymousAttribute = grainCallContext.InterfaceMethod.GetCustomAttribute<AllowAnonymousAttribute>()

        if allowAnonymousAttribute <> null then
            false
        else
            let mutable grainAuthorizeData: IEnumerable<IAuthorizeData> = null

            let grainMethodAuthorizeData =
                match grainCallContext.InterfaceMethod with
                | null -> null
                | _ -> grainCallContext.InterfaceMethod.GetCustomAttributes<AuthorizeAttribute>()
            if grainAuthorizeData <> null && not (grainAuthorizeData.Any()) && not (grainMethodAuthorizeData.Any()) then
                false
            else true
