namespace Orleans.Security

//open System.Threading.Tasks
//open Orleans.Runtime
//open System.Security.Claims
//open Orleans

//// TODO: Activation access violation. A non-activation thread attempted to access activation services
//type GrainWithClaimsPrincipal()=
//    [<DefaultValue>] val mutable User : ClaimsPrincipal 
//    inherit Orleans.Grain
//    interface IIncomingGrainCallFilter with
//        member this.Invoke(context: IIncomingGrainCallContext) = 
//            async {
//                this.User <- RequestContext.Get(ConfigurationKeys.ClaimsPrincipalKey) :?> ClaimsPrincipal
//                do! context.Invoke() |> Async.AwaitTask
//            } |> Async.StartAsTask :> Task