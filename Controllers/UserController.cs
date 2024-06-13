using API.Helpers;
using API.Shared;
using Apollo.API.DAL;
using Apollo.API.Models.DB;
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
    public class UserController : BaseController<User, ApolloContext>
    {
        public UserController(ApolloContext dbContext) : base(dbContext)
        {
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CheckEmailUsed([FromForm] int? id, [FromForm] string email)
        {
            bool result;
            if (id == null) //new item
            {
                result = await Repository.Query(false).Where(x => x.Email == email).AnyAsync();
            }
            else
            {
                result = await Repository.Query(false).Where(x => x.Id != id && x.Email == email).AnyAsync();
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public override async Task<IActionResult> Add([FromForm] string values)
        {
            var item = new User();
            JsonConvert.PopulateObject(values.ToString(), item);

            item.CreatedBy = GetUserID();

            if (!string.IsNullOrEmpty(item.Password))
            {
                item.Password = BCrypt.Net.BCrypt.HashPassword(item.Password);
            }
            else
            {
                throw new Exception("Password is required");
            }
            if (item.Status == 0)
            {
                item.Status = USER_STATUS.active;
            }
            var user = await Repository.Add(item);

            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] int userId, [FromForm] string newPassword)
        {
            var password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            var result = await Repository.Query(false)
                .Where(x => x.Id == userId)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.Password, password)
                );
            return Ok(result > 0);
        }

    }
}
