---
layout: default
title: Policy-based authorization
nav_order: 5
parent: Authorization
permalink: docs/authorization/policy-based-authorization
---

# Policy-based authorization

An authorization policy consists of one or more requirements. It’s registered as part of **`ClientBuilder.ConfigureServices`** and **`SiloHostBuilder.ConfigureServices`**, in the **`AddOrleansClusteringAuthorization`** method:

```csharp
.ConfigureServices(services =>
{
    services.AddOrleansClusteringAuthorization(identityServer4Info, options =>
    {
        options.AddPolicy("AdminPolicy", poliScy=> policy.RequireRole("Admin"));
    });
})
```

In the preceding example, an "AdminPolicy" policy is created.

Policies are applied by using the **`[Authorize]`** attribute with the policy name. For example:

```csharp
[Authorize(Policy = "AdminPolicy")]
public interface IUserGrain : IGrainWithStringKey
{
    Task<string> DoSomething();
}
```

## Requirements

An authorization requirement is a collection of data parameters that a policy can use to evaluate the current user/client principal. In our "EmailVerified" policy, the requirement is a single parameter—the email verified. A requirement implements IAuthorizationRequirement, which is an empty marker interface. A parameterized email verified requirement could be implemented as follows:

```csharp
using Orleans.Security.Clustering.Authorization;

public class EmailVerifiedRequirement : IAuthorizationRequirement
{
    public bool IsEmailVerified { get; private set; }

    public EmailVerifiedRequirement(bool isEmailVerified)
    {
        IsEmailVerified = isEmailVerified;
    }
}
```

Note: *a requirement doesn't need to have data or properties.*

## Authorization handlers

An authorization handler is responsible for the evaluation of a requirement's properties. The authorization handler evaluates the requirements against a provided AuthorizationHandlerContext to determine if access is allowed.

A requirement can have multiple handlers. A handler may inherit `AuthorizationHandler<TRequirement>`, where **`TRequirement`** is the requirement to be handled. Alternatively, a handler may implement `IAuthorizationHandler` to handle more than one type of requirement.

### **Use a handler for one requirement**

The following is an example of a one-to-one relationship in which a email verified handler utilizes a single requirement:

```csharp
public class EmailVerifiedHandler : AuthorizationHandler<EmailVerifiedRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        EmailVerifiedRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == JwtClaimTypes.EmailVerified))
        {
            var claim = context.User.FindFirst(c => c.Type == JwtClaimTypes.EmailVerified);
            var isEmailVerified = Convert.ToBoolean(claim.Value);

            if (isEmailVerified)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
```

The preceding code determines if the current user/client principal has an EmailVerified claim. Authorization can't occur when the claim is missing, in which case a completed task is returned. When a claim is present, the email verified flag is checked. If the user meets the minimum age defined by the requirement, authorization is deemed successful. When authorization is successful, context. Succeed is invoked with the satisfied requirement as its sole parameter.

### **Use a handler for multiple requirements**

The following is an example of a one-to-many relationship in which a permission handler utilizes two requirements:

```csharp
public class GenderRequirement : IAuthorizationRequirement
{
    public string Gender { get; private set; }

    public GenderRequirement(string gender)
    {
        Gender = gender;
    }
}

public class RoleIsPresentRequirement : IAuthorizationRequirement
{
    public string Role { get; private set; }

    public RoleIsPresentRequirement(string role)
    {
        Role = role;
    }
}

public class RoleAndGenderCombinationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var pendingRequirements = context.PendingRequirements.ToList();

        foreach (var requirement in pendingRequirements)
        {
            switch (requirement)
            {
                case RoleIsPresentRequirement roleIsPresentRequirement:
                {
                    if (context.User.IsInRole(roleIsPresentRequirement.Role))
                    {
                        context.Succeed(roleIsPresentRequirement);
                    }

                    break;
                }
                case GenderRequirement genderRequirement:
                {
                    if (context.User.HasClaim(c => c.Type == JwtClaimTypes.Gender))
                    {
                        var claim = context.User.FindFirst(c => c.Type == JwtClaimTypes.Gender)
                        if (claim.Value == genderRequirement.Gender)
                        {
                            context.Succeed(requirement);
                        }
                    }

                    break;
                }
            }
        }

        return Task.CompletedTask;
    }
}
```

The preceding code traverses PendingRequirements—a property containing requirements not marked as successful. When authorization is successful **`context.Succeed`** is invoked with the satisfied requirement as its sole parameter.

### Handler registration

Handlers are registered in the services collection during configuration. For example:

```csharp
.ConfigureServices(services =>
{
    services.AddOrleansClusteringAuthorization(identityServer4Info, options =>
    {
        options.AddPolicy("AdminPolicy", policy=> policy.RequireRole("Admin"));
    });

    services.AddSingleton<IAuthorizationHandler, EmailVerifiedHandler>();
    services.AddSingleton<IAuthorizationHandler, RoleAndGenderCombinationHandler>();
})
```

### What should a handler return?

Note that the **`Handle`** method in the handler example returns no value. How is a status of either success or failure indicated?

- A handler indicates success by calling **`context.Succeed(IAuthorizationRequirement requirement)`**, passing the requirement that has been successfully validated.
- A handler doesn't need to handle failures generally, as other handlers for the same requirement may succeed.
- To guarantee failure, even if other requirement handlers succeed, call **`context.Fail`**.

### Why would I want multiple handlers for a requirement?

In cases where you want evaluation to be on an **OR** basis, implement multiple handlers for a single requirement.

For the additional information please [see ASP.NET Core documentation](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-2.1#security-authorization-policies-based-handler-registration)