using System.Threading.Tasks;

namespace Orleans.Security.CoHosting
{
    public interface IAccessTokenProvider
    {
        Task<string> RetrieveTokenAsync();
    }
}
