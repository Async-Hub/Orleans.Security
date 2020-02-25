using System.Threading.Tasks;

namespace Orleans.Security
{
    public interface IAccessTokenProvider
    {
        Task<string> RetrieveTokenAsync();
    }
}
