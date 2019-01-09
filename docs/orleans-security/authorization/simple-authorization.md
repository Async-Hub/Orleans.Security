# Simple authorization in MS Orleans cluster

Authorization in Orleans is controlled through the **`AuthorizeAttribute`** attribute and its various parameters. At its simplest, applying the **`AuthorizeAttribute`** attribute to a **Grain Interface** or **Grain Interface Method** limits access to the grain or method to any authenticated user.

For example, the following code limits access to the **`UserGrain`** to any authenticated user.

```csharp
[Authorize]
public interface IUserGrain : IGrainWithStringKey
{
    Task<string> DoSomething();

    Task<string> DoSomethingElse();
}
```

If you want to apply authorization to a method rather than the grain, apply the **`AuthorizeAttribute`** attribute to the method itself:

```csharp
public interface IUserGrain : IGrainWithStringKey
{
    [Authorize]
    Task<string> DoSomething();

    Task<string> DoSomethingElse();
}
```

Now only authenticated users can access the **`DoSomething`** method and everyone can access **`DoSomethingElse`** method.

You can also use the **`AllowAnonymous`** attribute to allow access by non-authenticated users to individual actions. For example:

```csharp
[Authorize]
public interface IUserGrain : IGrainWithStringKey
{
    [AllowAnonymous]
    Task<string> DoSomething();

    Task<string> DoSomethingElse();
}
```

This would allow only authenticated users to the **`UserGrain`**, except for the **`DoSomething`** method, which is accessible by everyone, regardless of their authenticated or unauthenticated / anonymous status.

## Warning

**`[AllowAnonymous]` bypasses all authorization statements. If you combine `[AllowAnonymous]` and any `[Authorize]` attribute, the `[Authorize]` attributes are ignored. For example if you apply `[AllowAnonymous]` at the grain level, any `[Authorize]` attributes on the same grain (or on any method within it) is ignored.**