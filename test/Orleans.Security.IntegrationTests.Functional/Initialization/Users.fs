module Users

open IdentityModel
open IdentityServer4.Test
open System.Security.Claims

let getUsers() =
    // Alice user definition
    let aliceClaims =
        [| Claim(JwtClaimTypes.Name, "Alice Smith")
           Claim(JwtClaimTypes.GivenName, "Alice")
           Claim(JwtClaimTypes.FamilyName, "Smith")
           Claim(JwtClaimTypes.Email, "AliceSmith@email.com")
           Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
           Claim(JwtClaimTypes.WebSite, "http://alice.com")
           Claim(JwtClaimTypes.Role, "Admin")
           Claim(JwtClaimTypes.Role, "Manager") |]

    let alice = TestUser(SubjectId = "1", Username = "alice",
                         Password = "Pass123$", Claims = aliceClaims)

    // Bob user definition
    let bobClaims =
        [| Claim(JwtClaimTypes.Name, "Bob Smith")
           Claim(JwtClaimTypes.GivenName, "Bob")
           Claim(JwtClaimTypes.FamilyName, "Smith")
           Claim(JwtClaimTypes.Email, "BobSmith@email.com")
           Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
           Claim(JwtClaimTypes.WebSite, "http://bob.com")
           Claim(JwtClaimTypes.Role, "Developer") |]

    let bob = TestUser(SubjectId = "2", Username = "bob",
                       Password = "Pass123$", Claims = bobClaims)

    [ alice; bob ] |> ResizeArray<TestUser>
