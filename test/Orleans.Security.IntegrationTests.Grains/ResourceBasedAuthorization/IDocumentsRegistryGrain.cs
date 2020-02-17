using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
{
    [Authorize(Policy = "DocRegistryAccess")]
    public interface IDocumentsRegistryGrain : IGrainWithStringKey
    {
        Task Add(Document doc);
        
        Task<string> Modify(string docName, string newContent);

        Task<Document> Take(string docName);
    }
}