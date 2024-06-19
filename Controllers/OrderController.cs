using API.Helpers;
using API.Shared;
using Apollo.API.DAL;
using Apollo.API.Models.DB;
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
    public class OrderController : BaseController<Order, ApolloContext>
    {
        public OrderController(ApolloContext dbContext) : base(dbContext)
        {
        }


        [HttpGet]
        [Route("{id:int}")]
        public override async Task<IActionResult> GetById([FromRoute] int id)
        {
            var source = await Repository.Query(true).Include(x => x.Vehicle).FirstOrDefaultAsync(x => x.Id == id);
            return Ok(source);
        }



        [HttpGet]
        public override async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            //if (loadOptions.Take == 0 || loadOptions.Take > 100)
            //{
            //    throw new Exception("Can not fetch more than 100 items");
            //}
            var source = Repository.Query(includeCreatedBy: true).Include(x => x.Vehicle).Include(x => x.Provider);
            return Ok(DataSourceLoader.Load(source, loadOptions));
        }


        [HttpDelete]
        [Route("{key:int}")]
        public override async Task<IActionResult> Delete([FromRoute] int key)
        {
            throw new Exception("Delete is forbidden currently");
        }

        [HttpPost]
        public override async Task<IActionResult> Add([FromForm] string values)
        {
            var item = new Order();
            JsonConvert.PopulateObject(values.ToString(), item);

            if (!TryValidateModel(item))
                return BadRequest(ModelState);

            var vehicle = await Repository.DbContext.Vehicles.FirstOrDefaultAsync(x => x.Id == item.VehicleId);
            if (vehicle == null)
                return BadRequest("Vehicle not found");

            if (vehicle.PriceModel == PRICE_MODEL.perHour)
            {
                item.TotalCost = vehicle.PricePerModel * (decimal)((item.EndAt - item.StartAt)?.TotalHours ?? 0);
            }
            else if (vehicle.PriceModel == PRICE_MODEL.perKM)
            {
                item.TotalCost = vehicle.PricePerModel * item.Distance;
            }

            item.PriceModel = vehicle.PriceModel;
            item.PricePerModel = vehicle.PricePerModel;

            item.Status = ORDER_STATUS.created;
            item.ProviderId = vehicle.ProviderId;

            var customer = await Repository.DbContext.Customers.FirstOrDefaultAsync(x => x.UserId == GetUserID());
            if (customer == null)
                return BadRequest("Customer not found");
            item.CustomerId = customer.Id;

            item.CreatedBy = GetUserID();
            var newItem = await Repository.Add(item);
            return Ok(newItem);
        }


    }
}
