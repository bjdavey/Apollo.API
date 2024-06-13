using API.DAL;
using API.Helpers;
using Apollo.API.DAL;
using Apollo.API.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Apollo.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        protected BaseRepository<User, ApolloContext> Repository;
        protected Cache cache;
        private readonly IMemoryCache _memoryCache;

        public AuthenticationController(ApolloContext dbContext, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            this.Repository = new BaseRepository<User, ApolloContext>(dbContext);
            this.cache = new Cache(1000, Microsoft.Extensions.Caching.Memory.CacheItemPriority.High, 10, 1);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetTokenByPassword([FromForm] string email, [FromForm] string password)
        {
            var tmp = BCrypt.Net.BCrypt.HashPassword(password);
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            var user = await Repository.Query(false).SingleOrDefaultAsync(x => x.Email == email);
            if (user != null)
            {
                if (String.IsNullOrEmpty(user.Password))
                    return StatusCode(StatusCodes.Status401Unauthorized);

                var noOfAttempts = _memoryCache.GetOrCreate<int>("p-" + user.Id.ToString(),
                    cacheEntry =>
                    {
                        cacheEntry.SlidingExpiration = TimeSpan.FromHours(1);
                        return 0;
                    });

                if (noOfAttempts >= 5)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return Ok(GetLoginResponse(user));
                }
                else
                {
                    noOfAttempts++;
                    _memoryCache.Set("p-" + user.Id.ToString(), noOfAttempts, TimeSpan.FromHours(1));
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
        }

        private object GetLoginResponse(User user)
        {
            var claims = new List<Claim>()
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                            new Claim("Id", user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };
            if (!String.IsNullOrEmpty(user.Roles))
                foreach (var role in JsonConvert.DeserializeObject<string[]>(user.Roles))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

            return (new
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Roles = user.Roles,
                Token = Utilities.CreateToken(claims, DateTime.UtcNow.AddDays(1))
            });
        }


    }
}
