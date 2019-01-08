using System.Threading.Tasks;

namespace Orleans.Security.IntegrationTests.Extensions
{
    public class GrainAuthenticationFakeService //: IGrainAuthenticationService
    {
        public GrainAuthenticationFakeService()
        {
            AuthenticateResult = true;
        }

        private bool AuthenticateResult { get; }

        public Task<bool> Authenticate(string grainTypeName, IGrainFactory grainFactory)
        {
            return Task.FromResult(AuthenticateResult);
        }
    }
}