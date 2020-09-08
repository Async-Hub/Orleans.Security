namespace Orleans.Security.AccessToken

type AccessTokenVerifierOptions() =
    member val AllowOfflineValidation = false with get,set
    member val CacheEntryExpirationTime = 15 with get, set
    member val DisableCertificateValidation = false with get,set
    member val InMemoryCacheEnabled = false with get,set

//namespace Orleans.Security.AccessToken
//{
//    public class AccessTokenVerifierOptions
//    {
//        public bool AllowOfflineValidation { get; set; }
        
//        /// <summary>
//        /// Cache entry expiration time. The default value is 15 second.
//        /// </summary>
//        public int CacheEntryExpirationTime { get; set; } = 15;
        
//        public bool DisableCertificateValidation { get; set; }
        
//        public bool InMemoryCacheEnabled { get; set; }
//    }
//}