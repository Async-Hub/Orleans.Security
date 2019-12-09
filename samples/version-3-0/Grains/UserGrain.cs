using System;
using System.Threading.Tasks;
using GrainsInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Security;

namespace Grains
{
    public class UserGrain : Grain, IUserGrain
    {
        private readonly ILogger<IUserGrain> _logger;

        public UserGrain(ILogger<IUserGrain> logger)
        {
            _logger = logger;
        }

        public async Task<string> TakeSecret()
        {
            var grain = GrainFactory.GetGrain<IGlobalSecretStorageGrain>(nameof(IGlobalSecretStorageGrain));
            var userId = this.GetPrimaryKeyString();
            string secret;

            try
            {
                secret = await grain.TakeUserSecret(userId);
            }
            catch (OrleansClusterUnauthorizedAccessException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new UnauthorizedAccessException(ex.Message);
            }

            return secret;
        }
    }
}
