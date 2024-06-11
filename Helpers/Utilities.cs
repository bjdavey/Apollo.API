using Apollo.API;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Helpers
{
    public class Utilities
    {
        public static string CreateToken(List<Claim> claims, DateTime expire)
        {
            var IssuerSigningKey = Program.AppSettings.GetValue<string>("IssuerSigningKey");
            //var IssuerSigningKey = "*******";
            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: expire,
                notBefore: DateTime.UtcNow,
                audience: "Audience",
                issuer: "Issuer",
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IssuerSigningKey)),
                    SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string ComputeMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static bool CheckTokenIsValid(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var tokenTicks = long.Parse(tokenExp);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;
            var now = DateTime.Now.ToUniversalTime();
            var valid = tokenDate >= now;
            return valid;
        }

        public static async Task<HttpResponseMessage> GetRequest(string baseAddress, string requestUrl, AuthenticationHeaderValue authenticationHeaderValue = null)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);
                if (authenticationHeaderValue != null)
                    httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
                var response = await httpClient.GetAsync(requestUrl);
                return response;
            }
        }

        public static async Task<HttpResponseMessage> PostRequest(string baseAddress, string requestUrl, HttpContent httpContent, AuthenticationHeaderValue authenticationHeaderValue = null, Dictionary<string, string> headers = null)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);
                if (authenticationHeaderValue != null)
                    httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
                if (headers != null)
                {
                    headers.ToList().ForEach(x =>
                    {
                        httpClient.DefaultRequestHeaders.Add(x.Key, x.Value);
                    });
                }
                var response = await httpClient.PostAsync(requestUrl, httpContent);
                return response;
            }
        }

        public static async Task<HttpResponseMessage> PutRequest(string baseAddress, string requestUrl, HttpContent httpContent, AuthenticationHeaderValue authenticationHeaderValue = null)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);
                if (authenticationHeaderValue != null)
                    httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
                var response = await httpClient.PutAsync(requestUrl, httpContent);
                return response;
            }
        }

        public static StringContent JsonContent(object obj) => new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

        public static bool AddFile(string filePath, IFormFile file)
        {
            if (file != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return true;
            }
            else
                return false;
        }

    }
}
