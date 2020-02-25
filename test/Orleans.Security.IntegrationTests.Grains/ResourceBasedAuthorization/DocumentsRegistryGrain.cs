using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DocumentsRegistryGrain : GrainWithClaimsPrincipal, IDocumentsRegistryGrain
    {
        private readonly IPersistentState<List<Document>> _state;
        private readonly IAuthorizationService _authorizationService;

        public DocumentsRegistryGrain([PersistentState("state", 
            "MemoryGrainStorage")]IPersistentState<List<Document>> state,
            IAuthorizationService authorizationService)
        {
            _state = state;
            _authorizationService = authorizationService;
        }
        
        public async Task Add(Document doc)
        {
            _state.State.Add(doc);

            await _state.WriteStateAsync();
        }

        public async Task<string> Modify(string docName, string newContent)
        {
            var document = _state.State.Single(doc => doc.Name == docName);
            
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, document, AuthorizationConfig.DocumentModifyAccessPolicy);

            // ReSharper disable once InvertIf
            if (authorizationResult.Succeeded)
            {
                document.Content = newContent;
                await _state.WriteStateAsync();
                return document.Content;
            }

            return null;
        }

        public Task<Document> Take(string docName)
        {
            var document = _state.State.Single(doc => doc.Name == docName);

            return Task.FromResult(document);
        }
    }
}