module Users

open IdentityModel
open IdentityServer4.Test
open Orleans.Security.IntegrationTests.Grains.ClaimsBasedAuthorization
open Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
open System.Security.Claims

let aliceId = "1"
let bobId = "2"
let carolId = "3"

// Alice user definition
let private aliceClaims =
    [| Claim(JwtClaimTypes.Name, "Alice Smith")
       Claim(JwtClaimTypes.GivenName, "Alice")
       Claim(JwtClaimTypes.FamilyName, "Smith")
       Claim(JwtClaimTypes.Email, "AliceSmith@email.com")
       Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
       Claim(JwtClaimTypes.WebSite, "http://alice.com")
       Claim(JwtClaimTypes.Role, "Admin")
       Claim(JwtClaimTypes.Role, "Manager")
       Claim(DocRegistryAccessClaim.Name, DocRegistryAccessClaim.Value)
       Claim(CityClaim.Name, "New York") |]
let private alice = TestUser(SubjectId = aliceId, Username = "alice",
                     Password = "Pass123$", Claims = aliceClaims)
// Bob user definition
let private bobClaims =
    [| Claim(JwtClaimTypes.Name, "Bob Smith")
       Claim(JwtClaimTypes.GivenName, "Bob")
       Claim(JwtClaimTypes.FamilyName, "Smith")
       Claim(JwtClaimTypes.Email, "BobSmith@email.com")
       Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
       Claim(JwtClaimTypes.WebSite, "http://bob.com")
       Claim(JwtClaimTypes.Role, "Developer")
       Claim(DocRegistryAccessClaim.Name, DocRegistryAccessClaim.Value) |]
let private bob = TestUser(SubjectId = bobId, Username = "bob",
                   Password = "Pass123$", Claims = bobClaims)
// Carol user definition
let private carolClaims =
    [| Claim(JwtClaimTypes.Name, "Carol Smith")
       Claim(JwtClaimTypes.GivenName, "Carol")
       Claim(JwtClaimTypes.FamilyName, "Smith")
       Claim(JwtClaimTypes.Email, "CarolSmith@email.com")
       Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean)
       Claim(JwtClaimTypes.WebSite, "http://carol.com")
       Claim(JwtClaimTypes.Role, "Developer")
       Claim(JwtClaimTypes.Role, "Manager")
       Claim(CityClaim.Name, "Boston") |]
let private carol = TestUser(SubjectId = carolId, Username = "carol",
                     Password = "Pass123$", Claims = carolClaims)

let getUsers() =
    [ alice; bob; carol ] |> ResizeArray<TestUser>
