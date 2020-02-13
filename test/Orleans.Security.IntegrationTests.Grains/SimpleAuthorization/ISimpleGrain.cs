using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains.SimpleAuthorization
{
    [Authorize]
    public interface ISimpleGrain : IGrainWithGuidKey
    {
        Task<string> GetValue();
    }
}