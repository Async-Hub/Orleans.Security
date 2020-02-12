using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Orleans.Security.Authorization
{
    internal static class AuthorizationAdmission
    {
        public static bool IsRequired(IGrainCallContext grainCallContext)
        {
            var allowAnonymousAttribute = 
                grainCallContext.InterfaceMethod.GetCustomAttribute<AllowAnonymousAttribute>();
            
            // No authorization required.
            if (allowAnonymousAttribute != null)
            { 
                return false;
            }

            IEnumerable<IAuthorizeData> grainAuthorizeData = null;
            var grainMethodAuthorizeData = 
                grainCallContext.InterfaceMethod.GetCustomAttributes<AuthorizeAttribute>();

            if (grainCallContext.InterfaceMethod.ReflectedType != null)
            {
                grainAuthorizeData =
                    grainCallContext.InterfaceMethod.ReflectedType.GetCustomAttributes<AuthorizeAttribute>();
            }

            // No authorization required.
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (grainAuthorizeData != null && !grainAuthorizeData.Any() && !grainMethodAuthorizeData.Any())
            {
                return false;
            }

            return true;
        }
    }
}