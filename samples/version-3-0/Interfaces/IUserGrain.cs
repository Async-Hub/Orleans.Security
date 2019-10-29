using System.Threading.Tasks;
using Orleans;
using Orleans.Security.Authorization;

namespace GrainsInterfaces
{
    public interface IUserGrain : IGrainWithStringKey
    {
        [Authorize(Roles = "Admin")]
        Task<string> TakePrivateData();
    }
}
