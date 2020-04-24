namespace Orleans.Security.AccessToken

type AccessTokenVerifierOptions() =
    member val AllowOfflineValidation = false with get,set
    member val CacheEntryExpirationTime = 15 with get, set
    member val DisableCertificateValidation = false with get,set
    member val InMemoryCacheEnabled = false with get,set