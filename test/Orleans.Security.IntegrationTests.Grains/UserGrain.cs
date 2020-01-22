using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Orleans.Security.IntegrationTests.Grains
{
    public class UserGrain : Grain, IUserGrain
    {
        private readonly ILogger<IUserGrain> _logger;

        public UserGrain(ILogger<IUserGrain> logger)
        {
            _logger = logger;
        }

        public Task<string> GetWithAuthenticatedUser(string secret)
        {
            // var grain = GrainFactory.GetGrain<IGlobalSecretStorageGrain>(nameof(IGlobalSecretStorageGrain));
            // var userId = this.GetPrimaryKeyString();
            //
            // var secret = await grain.TakeUserSecret(userId);

            return Task.FromResult(secret);
        }

        public Task<string> GetWithAnonymousUser(string secret)
        {
            return Task.FromResult(secret);
        }
    }
}
