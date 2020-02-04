module Assembly

open Xunit

[<assembly: CollectionBehavior(DisableTestParallelization = true)>]
do()