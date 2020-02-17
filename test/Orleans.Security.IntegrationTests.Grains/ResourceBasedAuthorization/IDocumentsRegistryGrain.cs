using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
{
    [Authorize(Policy = "DocRegistryAccess")]
    public interface IDocumentsRegistryGrain : IGrainWithStringKey
    {
        //Task Add(string name, string content, string author)
        Task Add(Document document);
        
        Task<string> Modify(string content);

        Task<Document> Take(string name);
    }
}