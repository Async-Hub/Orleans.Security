namespace Orleans.Security.FSharp

open System.Reflection
open System.Runtime.CompilerServices

[<assembly: InternalsVisibleTo("Orleans.Security")>]
[<assembly: InternalsVisibleTo("Orleans.Security.Tests")>]
//TODO: Check why we need this.
[<assembly: AssemblyVersion("2.4.1.0")>]
[<assembly: AssemblyFileVersion("2.4.1.0")>]
[<assembly: AssemblyInformationalVersion("2.4.1")>]
do()