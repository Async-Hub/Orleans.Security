using System.Threading.Tasks;

namespace Orleans.Security.Client
{
    public interface IAccessTokenProvider
    {
        Task<string> RetrieveTokenAsync();
    }
}
