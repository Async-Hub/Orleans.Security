// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Security.Claims;

namespace Orleans.Security.Authorization
{
    /// <summary>
    /// A type used to provide a <see cref="AuthorizationHandlerContext"/> used for authorization.
    /// </summary>
    public interface IAuthorizationHandlerContextFactory
    {
        /// <summary>
        /// Creates a <see cref="AuthorizationHandlerContext"/> used for authorization.
        /// </summary>
        /// <param name="requirements">The requirements to evaluate.</param>
        /// <param name="user">The user to evaluate the requirements against.</param>
        /// <param name="resource">
        /// An optional resource the policy should be checked with.
        /// If a resource is not required for policy evaluation you may pass null as the value.
        /// </param>
        /// <returns>The <see cref="AuthorizationHandlerContext"/>.</returns>
        AuthorizationHandlerContext CreateContext(IEnumerable<IAuthorizationRequirement> requirements, ClaimsPrincipal user, object resource);
    }
}
