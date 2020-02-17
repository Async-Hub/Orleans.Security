using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DocumentsRegistryGrain : Grain, IDocumentsRegistryGrain
    {
        private readonly IPersistentState<List<Document>> _state;

        public DocumentsRegistryGrain([PersistentState("state", 
            "MemoryGrainStorage")]IPersistentState<List<Document>> state)
        {
            _state = state;
        }

        //public async Task Add(string name, string content, string author)
        public async Task Add(Document document)
        {
            //var doc = new Document(){Name = name, Author = author, Content = content};
            _state.State.Add(document);

            await _state.WriteStateAsync();
        }

        public Task<string> Modify(string content)
        {
            throw new System.NotImplementedException();
        }

        public Task<Document> Take(string name)
        {
            var doc = _state.State.Single(d => d.Name == name);

            return Task.FromResult(doc);
        }
    }
}