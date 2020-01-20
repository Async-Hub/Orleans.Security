using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains
{
    public interface IGlobalSecretStorageGrain : IGrainWithStringKey
    {
        [Authorize(Roles = "Admin")]
        Task<string> TakeUserSecret(string userId);
    }
}