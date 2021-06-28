namespace Orleans.Security.Authorization

open Orleans
open System.Reflection
open System.Linq

type internal AuthorizationAdmission() =
    static member IsRequired(grainCallContext: IGrainCallContext) =
        let allowAnonymousAttribute = grainCallContext.InterfaceMethod.GetCustomAttribute<AllowAnonymousAttribute>()

        if not (isNull allowAnonymousAttribute) then
            false
        else
            let grainAuthorizeData = 
                match grainCallContext.InterfaceMethod.ReflectedType with
                | null -> null
                | _ -> grainCallContext.InterfaceMethod.ReflectedType.GetCustomAttributes<AuthorizeAttribute>()

            let grainMethodAuthorizeData =
                match grainCallContext.InterfaceMethod with
                | null -> null
                | _ -> grainCallContext.InterfaceMethod.GetCustomAttributes<AuthorizeAttribute>()
            
            if grainAuthorizeData <> null && not (grainAuthorizeData.Any()) && not (grainMethodAuthorizeData.Any()) then
                false
            else true

//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace Orleans.Security.Authorization
//{
//    internal static class AuthorizationAdmission
//    {
//        public static bool IsRequired(IGrainCallContext grainCallContext)
//        {
//            var allowAnonymousAttribute = 
//                grainCallContext.InterfaceMethod.GetCustomAttribute<AllowAnonymousAttribute>();
            
//            // No authorization required.
//            if (allowAnonymousAttribute != null)
//            { 
//                return false;
//            }

//            IEnumerable<IAuthorizeData> grainAuthorizeData = null;
//            var grainMethodAuthorizeData = 
//                grainCallContext.InterfaceMethod.GetCustomAttributes<AuthorizeAttribute>();

//            if (grainCallContext.InterfaceMethod.ReflectedType != null)
//            {
//                grainAuthorizeData =
//                    grainCallContext.InterfaceMethod.ReflectedType.GetCustomAttributes<AuthorizeAttribute>();
//            }

//            // No authorization required.
//            // ReSharper disable once ConvertIfStatementToReturnStatement
//            if (grainAuthorizeData != null && !grainAuthorizeData.Any() && !grainMethodAuthorizeData.Any())
//            {
//                return false;
//            }

//            return true;
//        }
//    }
//}