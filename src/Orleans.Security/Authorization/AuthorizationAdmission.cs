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
            
            var grainMethodAuthorizeData = 
                grainCallContext.InterfaceMethod.GetCustomAttributes<AuthorizeAttribute>(); 
            
            IEnumerable<IAuthorizeData>  grainAuthorizeData =
                    grainCallContext.InterfaceMethod.ReflectedType?.GetCustomAttributes<AuthorizeAttribute>();

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