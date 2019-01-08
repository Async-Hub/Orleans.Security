namespace Orleans.Security.ClusterClient
{
    public interface IAccessTokenProvider
    {
        string RetrieveToken();
    }
}
