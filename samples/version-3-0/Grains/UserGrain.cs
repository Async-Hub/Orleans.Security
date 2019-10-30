using System.Threading.Tasks;
using GrainsInterfaces;
using Orleans;

namespace Grains
{
    public class UserGrain : Grain, IUserGrain
    {
        public Task<string> TakePrivateData()
        {
            return Task.FromResult($"{this.GetPrimaryKeyString()} super secret data from {nameof(UserGrain)}...");
        }
    }
}
