// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Orleans.Security.Authorization
{
    /// <summary>
    /// A type which can provide a <see cref="AuthorizationPolicy"/> for a particular name.
    /// </summary>
    public interface IAuthorizationPolicyProvider
    {
        /// <summary>
        /// Gets a <see cref="AuthorizationPolicy"/> from the given <paramref name="policyName"/>
        /// </summary>
        /// <param name="policyName">The policy name to retrieve.</param>
        /// <returns>The named <see cref="AuthorizationPolicy"/>.</returns>
        Task<AuthorizationPolicy> GetPolicyAsync(string policyName);

        /// <summary>
        /// Gets the default authorization policy.
        /// </summary>
        /// <returns>The default authorization policy.</returns>
        Task<AuthorizationPolicy> GetDefaultPolicyAsync();

        /// <summary>
        /// Gets the fallback authorization policy.
        /// </summary>
        /// <returns>The fallback authorization policy.</returns>
        Task<AuthorizationPolicy> GetFallbackPolicyAsync();
    }
}
