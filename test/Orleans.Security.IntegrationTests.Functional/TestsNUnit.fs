module TestsNUnit

open NUnit.Framework
open Orleans.Security.AccessToken

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test () =
    Assert.Pass()
