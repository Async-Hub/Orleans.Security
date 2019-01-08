using GrainsInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]/{id}")]
    public class UserController : Controller
    {
        private readonly IClusterClient _clusterClient;

        public UserController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpGet]
        public async Task<string> GetUser()
        {
            var grain = _clusterClient.GetGrain<IUserGrain>("Alice");
            var result = await grain.TakePrivateData();

            return result;
        }
    }
}
