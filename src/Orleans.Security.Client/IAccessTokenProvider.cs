namespace Orleans.Security.Client
{
    public interface IAccessTokenProvider
    {
        string RetrieveToken();
    }
}
