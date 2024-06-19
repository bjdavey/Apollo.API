using API.Helpers;
using Apollo.API.DAL;
using Apollo.API.Models.DB;
using Apollo.API.Services;
using DevExtreme.API.Datasource;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Apollo.API.Controllers
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
        public async Task<IActionResult> GetDetails([FromRoute] int id = -1)
        {
            var vehicle = await Repository.Query(true).Include(x => x.VehicleRestriction).FirstOrDefaultAsync(x => x.Id == id);
            var device = await Traccar.FetchDevice(vehicle.DeviceUnique);
            return Ok(new
            {
                vehicle,
                device
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = Repository.Query(includeCreatedBy: true).Include(x => x.VehicleRestriction);
            var res = from vehicle in vehicles
                      select new
                      {
                          vehicle,
                          device = Traccar.FetchDevice(vehicle.DeviceUnique).Result
                      };
            return Ok(res);
        }


        [HttpGet]
        [Route("{id:int}")]
        public override async Task<IActionResult> GetById([FromRoute] int id)
        {
            var source = await Repository.Query(true).Include(x => x.VehicleRestriction).FirstOrDefaultAsync(x => x.Id == id);
            return Ok(source);
        }

        [HttpGet]
        public override async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            //if (loadOptions.Take == 0 || loadOptions.Take > 100)
            //{
            //    throw new Exception("Can not fetch more than 100 items");
            //}
            var source = Repository.Query(includeCreatedBy: true).Include(x => x.VehicleRestriction);
            return Ok(DataSourceLoader.Load(source, loadOptions));
        }


        //[HttpPost]
        //public override async Task<IActionResult> Add([FromForm] string values)
        //{
        //    var vehicle = new Vehicle();
        //    JsonConvert.PopulateObject(values.ToString(), vehicle);
        //    //var vehicleRestriction = new VehicleRestriction();
        //    //JsonConvert.PopulateObject(values, vehicleRestriction);
        //    //vehicle.VehicleRestriction = vehicleRestriction;

        //    if (!TryValidateModel(vehicle))
        //        return BadRequest(ModelState);

        //    var device = Traccar.CreateDevice(vehicle.Title, vehicle.DeviceUnique);
        //    if (device == null)
        //    {
        //        throw new Exception("Error in creating the device");
        //    }

        //    vehicle.DeviceId = device.Id;
        //    vehicle.CreatedBy = GetUserID();
        //    var newItem = await Repository.Add(vehicle);
        //    return Ok(newItem);
        //}

        //[HttpPut]
        //public override async Task<IActionResult> Update([FromForm] int key, [FromForm] string values)
        //{
        //    var item = await Repository.Query(false).Include(x => x.VehicleRestriction).FirstOrDefaultAsync(x => x.Id == key);
        //    if (item == null)
        //        return Ok(false);
        //    JsonConvert.PopulateObject(values, item);

        //    var res = (await Repository.DbContext.SaveChangesAsync() > 0);
        //    return Ok(res);
        //}


        [HttpPost]
        public async Task<IActionResult> AddWithFile([FromForm] string values, [FromForm] IFormFile file)
        {
            var vehicle = new Vehicle();
            JsonConvert.PopulateObject(values.ToString(), vehicle);

            if (!TryValidateModel(vehicle))
                return BadRequest(ModelState);

            var device = Traccar.CreateDevice(vehicle.Title, vehicle.DeviceUnique);
            if (device == null)
            {
                throw new Exception("Error in creating the device");
            }

            vehicle.DeviceId = device.Id;

            var provider = await Repository.DbContext.Providers.FirstOrDefaultAsync(x => x.UserId == GetUserID());
            if (provider == null)
                return BadRequest("Provider not found");
            vehicle.ProviderId = provider.Id;

            vehicle.CreatedBy = GetUserID();
            if (file != null)
            {
                var fileName = vehicle.Id + "-" + Guid.NewGuid().ToString("N") + ".png";
                var filePath = Path.Combine($"Files/Vehicles/{fileName}");
                if (Utilities.AddFile(filePath, file))
                    vehicle.Picture = fileName;
            }
            var newItem = await Repository.Add(vehicle);
            return Ok(newItem);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWithFile([FromForm] int key, [FromForm] string values, [FromForm] IFormFile file)
        {
            var item = await Repository.Query(false).Include(x => x.VehicleRestriction).FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                return Ok(false);
            JsonConvert.PopulateObject(values, item);

            if (file != null)
            {
                var fileName = item.Id + "-" + Guid.NewGuid().ToString("N") + ".png";
                var filePath = Path.Combine($"Files/Vehicles/{fileName}");
                if (Utilities.AddFile(filePath, file))
                    item.Picture = fileName;
            }
            var res = (await Repository.DbContext.SaveChangesAsync() > 0);
            return Ok(res);
        }

        [HttpDelete]
        [Route("{key:int}")]
        public override async Task<IActionResult> Delete([FromRoute] int key)
        {
            throw new Exception("Delete is forbidden currently");
        }

    }
}
