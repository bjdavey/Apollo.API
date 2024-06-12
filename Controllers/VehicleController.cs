using API.Helpers;
using Apollo.API.DAL;
using Apollo.API.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Qi.CDC.API.Controllers
{
    //[Authorize(Policy = "SysAdmin")]
    //[Authorize(Roles = Roles.Users)]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class VehicleController : BaseController<Vehicle, ApolloContext>
    {
        public VehicleController(ApolloContext dbContext) : base(dbContext)
        {
        }

        [HttpDelete]
        [Route("{key:int}")]
        public override async Task<IActionResult> Delete([FromRoute] int key)
        {
            throw new Exception("Delete is forbidden currently");
        }

    }
}
