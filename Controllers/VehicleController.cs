using API.Helpers;
using Apollo.API.DAL;
using Apollo.API.Models.DB;
using Apollo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers
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

        [HttpPost]
        public async Task<IActionResult> CheckDeviceUniqueUsed([FromForm] int? id, [FromForm] string deviceUnique)
        {
            bool result;
            if (id == null) //new item
            {
                result = await Repository.Query(false).Where(x => x.DeviceUnique == deviceUnique).AnyAsync();
            }
            else
            {
                result = await Repository.Query(false).Where(x => x.Id != id && x.DeviceUnique == deviceUnique).AnyAsync();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDetails([FromRoute] int id)
        {
            var vehicle = await Repository.Query(true).Include(x => x.VehicleRestriction).FirstOrDefaultAsync(x => x.Id == id);
            var device = await Traccar.FetchDevice(vehicle.DeviceUnique);
            return Ok(new
            {
                vehicle,
                device
            });
        }

        [HttpPost]
        public override async Task<IActionResult> Add([FromForm] string values)
        {
            var item = new Vehicle();
            JsonConvert.PopulateObject(values.ToString(), item);

            if (!TryValidateModel(item))
                return BadRequest(ModelState);

            var device = Traccar.CreateDevice(item.Title, item.DeviceUnique);
            if (device == null)
            {
                throw new Exception("Error in creating the device");
            }

            item.DeviceId = device.Id;
            item.CreatedBy = GetUserID();
            var newItem = await Repository.Add(item);
            return Ok(newItem);
        }

        [HttpDelete]
        [Route("{key:int}")]
        public override async Task<IActionResult> Delete([FromRoute] int key)
        {
            throw new Exception("Delete is forbidden currently");
        }

    }
}
