using API.DAL;
using DevExtreme.API.Datasource;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace API.Helpers
{
    [Route("api/[controller]/[action]")]
    public abstract class BaseController<TEntity, TDbContext> : Controller
        where TEntity : EntityBase, new()
        where TDbContext : DbContext, new()
    {
        protected BaseRepository<TEntity, TDbContext> Repository;
        public BaseController(TDbContext dbContext)
        {
            this.Repository = new BaseRepository<TEntity, TDbContext>(dbContext);
        }

        protected int? GetUserID()
        {
            var ident = User.Identity as ClaimsIdentity;
            var id = ident.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            return id == null ? null : Convert.ToInt32(id);
        }
        //protected UserType? GetUserType()
        //{
        //    var ident = User.Identity as ClaimsIdentity;
        //    var type = ident.Claims.FirstOrDefault(c => c.Type == "Type")?.Value;
        //    return type == null ? null : (UserType)Enum.Parse(typeof(UserType), type, true);
        //}


        [HttpGet]
        [Route("{id:int}")]
        public virtual async Task<IActionResult> GetById([FromRoute] int id)
        {
            var source = await Repository.Query(true).FirstOrDefaultAsync(x => x.Id == id);
            return Ok(source);
        }


        [HttpGet]
        public virtual async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            //if (loadOptions.Take == 0 || loadOptions.Take > 100)
            //{
            //    throw new Exception("Can not fetch more than 100 items");
            //}
            var source = Repository.Query(includeCreatedBy: true);
            return Ok(DataSourceLoader.Load(source, loadOptions));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add([FromForm] string values)
        {
            var item = new TEntity();
            JsonConvert.PopulateObject(values.ToString(), item);

            if (!TryValidateModel(item))
                return BadRequest(ModelState);

            item.CreatedBy = GetUserID();
            var newItem = await Repository.Add(item);
            return Ok(newItem);
        }


        [HttpDelete]
        [Route("{key:int}")]
        public virtual async Task<IActionResult> Delete([FromRoute] int key)
        {
            var res = await Repository.Delete(key);
            return Ok(res);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update([FromForm] int key, [FromForm] string values)
        {
            var res = await Repository.Update(key, values);
            return Ok(res);
        }

        // Now, I wish I could write you a melody so plain,
        // That could ease you and cool you and cease the pain 
        // Of your useless and pointless knowledge



    }

}