namespace Orleans.Security.AccessToken

open System
open System.Security.Cryptography
open System.Text
open System.Threading.Tasks
open Orleans.Security.Caching

module Hashing =
    let tryToOptimize accessToken =
        let accessTokenType = AccessTokenAnalyzer.GetTokenType accessToken
        if accessTokenType = AccessTokenType.Reference then accessToken
        else
            use sha256 = SHA256.Create()
            let hashValue = Encoding.UTF8.GetBytes(accessToken) |> sha256.ComputeHash
            let stringBuilder = new StringBuilder()
            hashValue |> Seq.iter (fun elem -> 
                stringBuilder.Append(elem.ToString("x2"))|> ignore)


            stringBuilder.ToString()

type AccessTokenVerifierWithCaching(accessTokenVerifier: IAccessTokenVerifier,
                                    accessTokenCache: IAccessTokenCache, 
                                    cacheEntryExpirationTime: int) =
    interface IAccessTokenVerifier with
        member _.Verify accessToken = 
            async {
                //if String.IsNullOrWhiteSpace accessToken then raise(ArgumentException(nameof accessToken))
                if String.IsNullOrWhiteSpace accessToken then raise(ArgumentException("accessToken"))
                // TODO: Need deeper investigations, maybe this optimization is unnecessary.
                let key = Hashing.tryToOptimize accessToken

                let mutable cacheValue : obj = null
                if accessTokenCache.Current.TryGetValue(key, &cacheValue) then
                    return cacheValue :?> AccessTokenVerificationResult
                else
                    let! verificationResult = 
                        accessTokenVerifier.Verify(accessToken) |> Async.AwaitTask

                    let cacheEntry = accessTokenCache.Current.CreateEntry(key)
                    cacheEntry.Value <- verificationResult
                    cacheEntry.AbsoluteExpiration <- 
                        Nullable<DateTimeOffset>(DateTimeOffset.Now.AddSeconds(Convert.ToDouble(cacheEntryExpirationTime)))

                    return verificationResult } |> Async.StartAsTask

//using System;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using Orleans.Security.Caching;

//namespace Orleans.Security.AccessToken
//{
//    internal class AccessTokenVerifierWithCaching : IAccessTokenVerifier
//    {
//        private readonly IAccessTokenCache _accessTokenCache;
//        private readonly int _cacheEntryExpirationTime;

//        private readonly IAccessTokenVerifier _accessTokenVerifier;

//        public AccessTokenVerifierWithCaching(IAccessTokenVerifier accessTokenVerifier, 
//            IAccessTokenCache accessTokenCache, int cacheEntryExpirationTime)
//        {
//            _accessTokenVerifier = accessTokenVerifier;
//            _accessTokenCache = accessTokenCache;
//            _cacheEntryExpirationTime = cacheEntryExpirationTime;
//        }

//        public async Task<AccessTokenVerificationResult> Verify(string accessToken)
//        {
//            if (string.IsNullOrWhiteSpace(accessToken))
//            {
//                throw new ArgumentException($"The value of {nameof(accessToken)} can't be null or empty.");
//            }

//            // TODO: Need deeper investigations, maybe this optimization is unnecessary.
//            var key = TryToOptimize(accessToken);

//            if (_accessTokenCache.Current.TryGetValue(key, out var result))
//            {
//                return result as AccessTokenVerificationResult;
//            }

//            var verificationResult = await _accessTokenVerifier.Verify(accessToken);

//            var cacheEntry = _accessTokenCache.Current.CreateEntry(key);
//            cacheEntry.Value = verificationResult;
//            cacheEntry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(_cacheEntryExpirationTime);

//            return verificationResult;
//        }

//        private string TryToOptimize(string accessToken)
//        {
//            var accessTokenType = AccessTokenAnalyzer.GetTokenType(accessToken);

//            if (accessTokenType == AccessTokenType.Reference) return accessToken;

//            using (var sha256 = SHA256.Create())
//            {
//                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(accessToken));

//                var sb = new StringBuilder();

//                // ReSharper disable once ForCanBeConvertedToForeach
//                for (int i = 0; i < hashValue.Length; i++)
//                {
//                    sb.Append(hashValue[i].ToString("x2"));
//                }

//                return sb.ToString();
//            }
//        }
//    }
//}