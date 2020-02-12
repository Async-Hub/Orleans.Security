using System.Threading.Tasks;
using Orleans;
using Orleans.Security.Authorization;

namespace Grains
{
    public interface IGlobalSecretStorageGrain : IGrainWithStringKey
    {
        [Authorize(Roles = "Admin")]
        Task<string> TakeUserSecret(string userId);
    }
}