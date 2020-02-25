using System.Threading.Tasks;

namespace Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
{
    public class UserGrain : Grain, IUserGrain
    {
        public async Task<string> GetDocumentContent(string docName)
        {
            var grain = GrainFactory.GetGrain<IDocumentsRegistryGrain>(DocumentsRegistry.Default);
            var doc = await grain.Take(docName);
            
            return doc.Content;
        }

        public async Task<string> ModifyDocumentContent(string docName, string newContent)
        {
            var grain = GrainFactory.GetGrain<IDocumentsRegistryGrain>(DocumentsRegistry.Default);

            return await grain.Modify(docName, newContent);
        }
    }
}
