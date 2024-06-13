using API.Helpers;
using Apollo.API.DAL;
using Apollo.API.Models.DB;
using DevExtreme.API.Datasource;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Apollo.API.Controllers
{
    //[Authorize(Policy = "SysAdmin")]
    //[Authorize(Roles = Roles.Users)]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class CustomerController : BaseController<Customer, ApolloContext>
    {
        public CustomerController(ApolloContext dbContext) : base(dbContext)
        {
        }


        [HttpGet]
        [Route("{id:int}")]
        public override async Task<IActionResult> GetById([FromRoute] int id)
        {
            var source = await Repository.Query(true).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            return Ok(source);
        }



        [HttpGet]
        public override async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            //if (loadOptions.Take == 0 || loadOptions.Take > 100)
            //{
            //    throw new Exception("Can not fetch more than 100 items");
            //}
            var source = Repository.Query(includeCreatedBy: true).Include(x => x.User);
            return Ok(DataSourceLoader.Load(source, loadOptions));
        }


        [HttpDelete]
        [Route("{key:int}")]
        public override async Task<IActionResult> Delete([FromRoute] int key)
        {
            throw new Exception("Delete is forbidden currently");
        }

    }
}
