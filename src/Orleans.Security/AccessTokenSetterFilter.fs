namespace Orleans.Security

//open System
//open Orleans.Runtime
//open Orleans.Security.Extensions
//open Orleans.Security.Authorization
//open Orleans
//open System.Threading.Tasks

//type internal AccessTokenSetterFilter(accessTokenProvider: IAccessTokenProvider)=
//    let accessTokenProvider = accessTokenProvider
//    interface IOutgoingGrainCallFilter with
//        member _.Invoke (context: IOutgoingGrainCallContext) =
//                async {
//                    if AuthorizationAdmission.IsRequired(context) then
//                        let accessToken = (RequestContext.Get(ConfigurationKeys.AccessTokenKey) |?
//                                                    (String.Empty :> Object)).ToString()
                    
//                        if String.IsNullOrWhiteSpace(accessToken) then
//                            let! newAccessToken = accessTokenProvider.RetrieveTokenAsync() |> Async.AwaitTask
                        
//                            if String.IsNullOrWhiteSpace(newAccessToken) then
//                                raise (InvalidOperationException("AccessToken can not be null or empty."))
                        
//                            RequestContext.Set(ConfigurationKeys.AccessTokenKey, newAccessToken);
                    
//                    context.Invoke() |> Async.AwaitTask |> ignore
//                } |> Async.StartAsTask :> Task
                
// using System;
// using System.Threading.Tasks;
// using Orleans.Runtime;
// using Orleans.Security.Authorization;
//
// namespace Orleans.Security
// {
//     internal class AccessTokenSetterFilter : IOutgoingGrainCallFilter
//     {
//         private readonly IAccessTokenProvider _accessTokenProvider;
//
//         public AccessTokenSetterFilter(IAccessTokenProvider accessTokenProvider)
//         {
//             _accessTokenProvider = accessTokenProvider;
//         }
//
//         public async Task Invoke(IOutgoingGrainCallContext context)
//         {
//             if (AuthorizationAdmission.IsRequired(context))
//             {
//                 var accessToken = RequestContext.Get(ConfigurationKeys.AccessTokenKey)?.ToString();
//
//                 if (string.IsNullOrWhiteSpace(accessToken))
//                 {
//                     accessToken = await _accessTokenProvider.RetrieveTokenAsync();
//
//                     if (string.IsNullOrWhiteSpace(accessToken))
//                     {
//                         throw new InvalidOperationException("AccessToken can not be null or empty.");
//                     }
//
//                     RequestContext.Set(ConfigurationKeys.AccessTokenKey, accessToken);
//                 }
//             }
//
//             await context.Invoke();
//         }
//     }
// }