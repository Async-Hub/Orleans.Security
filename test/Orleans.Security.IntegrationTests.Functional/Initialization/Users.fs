module Users

open IdentityModel
open IdentityServer4.Test
open System.Security.Claims

let aliceEmail = "AliceSmith@email.com"
let bobEmail = "AliceSmith@email.com"
let carolEmail = "BobSmith@email.com"

let documentRegistryAccess = "DocRegistryAccess";

// Alice user definition
let private aliceClaims =
    [| Claim(JwtClaimTypes.Name, "Alice Smith")
       Claim(JwtClaimTypes.GivenName, "Alice")
       Claim(JwtClaimTypes.FamilyName, "Smith")
       Claim(JwtClaimTypes.Email, aliceEmail)
       Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
       Claim(JwtClaimTypes.WebSite, "http://alice.com")
       Claim(JwtClaimTypes.Role, "Admin")
       Claim(JwtClaimTypes.Role, "Manager")
       Claim(documentRegistryAccess, documentRegistryAccess) |]
let private alice = TestUser(SubjectId = "1", Username = "alice",
                     Password = "Pass123$", Claims = aliceClaims)
// Bob user definition
let private bobClaims =
    [| Claim(JwtClaimTypes.Name, "Bob Smith")
       Claim(JwtClaimTypes.GivenName, "Bob")
       Claim(JwtClaimTypes.FamilyName, "Smith")
       Claim(JwtClaimTypes.Email, bobEmail)
       Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
       Claim(JwtClaimTypes.WebSite, "http://bob.com")
       Claim(JwtClaimTypes.Role, "Developer")
       Claim(documentRegistryAccess, documentRegistryAccess) |]
let private bob = TestUser(SubjectId = "2", Username = "bob",
                   Password = "Pass123$", Claims = bobClaims)
// Carol user definition
let private carolClaims =
    [| Claim(JwtClaimTypes.Name, "Carol Smith")
       Claim(JwtClaimTypes.GivenName, "Carol")
       Claim(JwtClaimTypes.FamilyName, "Smith")
       Claim(JwtClaimTypes.Email, carolEmail)
       Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean)
       Claim(JwtClaimTypes.WebSite, "http://carol.com")
       Claim(JwtClaimTypes.Role, "Developer")
       Claim(JwtClaimTypes.Role, "Manager") |]
let private carol = TestUser(SubjectId = "3", Username = "carol",
                     Password = "Pass123$", Claims = carolClaims)

let getUsers() =
    [ alice; bob; carol ] |> ResizeArray<TestUser>
