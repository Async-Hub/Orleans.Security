using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
{
    [Authorize]
    public interface IUserGrain : IGrainWithStringKey
    {
        Task<string> GetDocumentContent(string docName);

        Task<string> ModifyDocumentContent(string docName, string newContent);
    }
}
