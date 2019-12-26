using System.Collections.Generic;
using System.Threading.Tasks;
using GrainsInterfaces;
using Orleans;

namespace Grains
{
    public class GlobalSecretStorageGrain : Grain, IGlobalSecretStorageGrain
    {
        public Task<string> TakeUserSecret(string userId)
        {
            var secret = Secrets.GetUserSecret(userId);

            return Task.FromResult(secret);
        }

        private static class Secrets
        {
            private static readonly Dictionary<string, string> UsersSecrets;

            static Secrets()
            {
                UsersSecrets = new Dictionary<string, string>
                {
                    {"Alice", "Alice super secret data..."}, 
                    {"Bob", "Bob super secret data..."}
                };
            }

            public static string GetUserSecret(string userId) => UsersSecrets[userId];
        }
    }
}