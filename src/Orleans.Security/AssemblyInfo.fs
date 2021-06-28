namespace Orleans.Security.FSharp

open System.Reflection
open System.Runtime.CompilerServices

[<assembly: InternalsVisibleTo("Orleans.Security.Client")>]
[<assembly: InternalsVisibleTo("Orleans.Security.Clustering")>]
[<assembly: InternalsVisibleTo("Orleans.Security.Interoperability")>]
[<assembly: InternalsVisibleTo("Orleans.Security.Tests")>]
//TODO: Check why we need this.
[<assembly: AssemblyVersion("1.3.0.0")>]
[<assembly: AssemblyFileVersion("1.3.0.0")>]
[<assembly: AssemblyInformationalVersion("1.3.0.0")>]
do()