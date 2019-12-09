using System;
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
        public async Task<ActionResult<string>> GetUser(string id)
        {
            var grain = _clusterClient.GetGrain<IUserGrain>(id);

            try
            {
                return await grain.TakeSecret();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Access to the requested resource denied.");
            }
        }
    }
}
