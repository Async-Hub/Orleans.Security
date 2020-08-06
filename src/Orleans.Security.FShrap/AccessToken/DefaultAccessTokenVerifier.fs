namespace Orleans.Security.AccessToken

open System

type internal DefaultAccessTokenVerifier(options: AccessTokenVerifierOptions,
                                         introspectionService: IAccessTokenIntrospectionService) =
    interface IAccessTokenVerifier with
        member this.Verify accessToken =
            async {
                if String.IsNullOrWhiteSpace accessToken then
                    raise (ArgumentException("The value of accessToken can't be null or empty."))

                let! introspectionResult = introspectionService.IntrospectTokenAsync accessToken
                                               options.AllowOfflineValidation |> Async.AwaitTask
                if introspectionResult.IsValid then
                    return AccessTokenVerificationResult.CreateSuccess
                               (System.Nullable(introspectionResult.AccessTokenType), introspectionResult.Claims)
                else return AccessTokenVerificationResult.CreateFailed(introspectionResult.Message)
            }
            |> Async.StartAsTask

//using System;
//using System.Threading.Tasks;

//namespace Orleans.Security.AccessToken
//{
//    // ReSharper disable once ClassNeverInstantiated.Global
//    internal class DefaultAccessTokenVerifier : IAccessTokenVerifier
//    {
//        private readonly AccessTokenVerifierOptions _options;

//        private readonly IAccessTokenIntrospectionService _introspectionService;

//        public DefaultAccessTokenVerifier(AccessTokenVerifierOptions options,
//            IAccessTokenIntrospectionService introspectionService)
//        {
//            _options = options;
//            _introspectionService = introspectionService;
//        }

//        public async Task<AccessTokenVerificationResult> Verify(string accessToken)
//        {
//            if (string.IsNullOrWhiteSpace(accessToken))
//            {
//                throw new ArgumentException($"The value of {nameof(accessToken)} can't be null or empty.");
//            }

//            var introspectionResult =
//                await _introspectionService.IntrospectTokenAsync(accessToken, _options.AllowOfflineValidation);

//            // ReSharper disable once InvertIf
//            if (introspectionResult.IsValid)
//            {
//                return AccessTokenVerificationResult.CreateSuccess(introspectionResult.AccessTokenType, 
//                    introspectionResult.Claims);
//            }

//            return AccessTokenVerificationResult.CreateFailed(introspectionResult.Message);
//        }
//    }
//}