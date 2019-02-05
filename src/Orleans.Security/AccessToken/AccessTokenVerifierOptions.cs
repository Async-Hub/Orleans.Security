namespace Orleans.Security.AccessToken
{
    public class AccessTokenVerifierOptions
    {
        public bool InMemoryCacheEnabled { get; set; }


        public bool DisableCertificateValidation { get; set; }

        /// <summary>
        /// Cache entry expiration time. The default value is 30 seccond.
        /// </summary>
        public int CacheEntryExpirationTime { get; set; } = 30;
    }
}