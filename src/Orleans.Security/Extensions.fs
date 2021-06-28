namespace Orleans.Security

open System.Threading.Tasks

module Extensions =
    let (|?) lhs rhs = (if isNull lhs then rhs else lhs)

    type Async with
        static member AwaitTaskAndTryToUnwrapException (task: Task) = 
            async {
                try
                    do! task |> Async.AwaitTask
                with
                | :? System.AggregateException as ex -> 
                    if (not (isNull ex.InnerExceptions)) && ex.InnerExceptions.Count = 1 then
                        raise ex.InnerException
            }
    
