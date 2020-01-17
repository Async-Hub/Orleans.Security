---
layout: default
title: Role-based authorization
nav_order: 3
parent: Authorization
permalink: docs/authorization/role-based-authorization
---

# Role-based authorization

When an identity is created it may belong to one or more roles. For example, Alice may belong to the Administrator and User roles whilst Bob may only belong to the User role. How these roles are created and managed depends on the IdentityServer4 of the authorization process.

Role-based authorization checks are declarative—the developer embeds them within their code, against a grain interface or an method within a grain interface, specifying roles which the current *user* or *client* (please [see](http://docs.identityserver.io/en/latest/intro/terminology.html) IdentityServer4 terminology) must be a member of to access the requested resource.

For example, the following code limits access to any methods on the implementation of **`IUserGrain`** to users/clients who are a member of the Administrator role:

```csharp
[Authorize(Roles = "Administrator")]
public interface IUserGrain : IGrainWithStringKey
{
    Task<string> DoSomething();

    Task<string> DoSomethingElse();
}
```

You can specify multiple roles as a comma separated list:

```csharp
[Authorize(Roles = "Administrator, Manager")]
public interface IUserGrain : IGrainWithStringKey
{
    Task<string> DoSomething();

    Task<string> DoSomethingElse();
}
```

This grain would be only accessible by users/clients who are members of the `Administrator` role or the `Manager` role.

If you apply multiple attributes then an accessing user/client must be a member of all the roles specified; the following sample requires that a user must be a member of both the `Developer` and `Manager` role.

```csharp
[Authorize(Roles = "Developer")]
[Authorize(Roles = "Manager")]
public interface IUserGrain : IGrainWithStringKey
{
    Task<string> DoSomething();

    Task<string> DoSomethingElse();
}
```

You can further limit access by applying additional role authorization attributes at the method level:

```csharp
[Authorize(Roles = "Developer")]
[Authorize(Roles = "Manager")]
public interface IUserGrain : IGrainWithStringKey
{
    Task<string> DoSomething();

    [Authorize(Roles = "Manager")]
    Task<string> DoSomethingElse();
}
```

In the previous code snippet members of the `Developer` role or the `Manager` role can access the grain and the **`DoSomething`** method, but only members of the `Manager` role can access the **`DoSomethingElse`** method.

You can also lock down a grain but allow anonymous, unauthenticated access to individual methods.

```csharp
[Authorize]
public interface IUserGrain : IGrainWithStringKey
{
    Task<string> DoSomething();

    [AllowAnonymous]
    Task<string> DoSomethingElse();
}
```