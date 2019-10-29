using System;
using System.Linq;
using System.Threading.Tasks;

namespace Orleans.Security
{
    internal static class RuntimeMethodBinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullyQualifiedNameOfType">"[FullTypeName, AssemblyName]"</param>
        /// <param name="nameOfMethod"></param>
        /// <param name="arguments"></param>
        /// <param name="argumentsCount"></param>
        /// <returns></returns>
        public static async Task<dynamic> InvokeAsync(string fullyQualifiedNameOfType,
            string nameOfMethod, object[] arguments, int argumentsCount = 0)
        {
            var type = Type.GetType(fullyQualifiedNameOfType, true);

            var methodQueryPredicate = type.GetMethods().Where(methodInfo => methodInfo.Name.Contains(nameOfMethod));

            methodQueryPredicate = methodQueryPredicate.Where(methodInfo => methodInfo.GetParameters().Length == argumentsCount);

            var method = methodQueryPredicate.FirstOrDefault();

            if (method == null)
            {
                throw new InvalidOperationException($"The appropriate method {nameOfMethod}" +
                                                    " doesn't find in IdentityModel package.");
            }

            var result = await (dynamic)method.Invoke(null, arguments);

            return result;
        }
    }
}